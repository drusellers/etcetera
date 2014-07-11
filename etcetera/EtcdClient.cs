namespace etcetera
{
    using System;
    using System.Linq;
    using System.Text;
    using RestSharp;
    using System.Security.Cryptography.X509Certificates;

    public class EtcdClient : IEtcdClient
    {
        readonly IRestClient _client;
        readonly Uri _keysRoot;

        public EtcdClient(Uri etcdLocation)
        {
            var uriBuilder = new UriBuilder(etcdLocation)
            {
                Path = ""
            };
            var root = uriBuilder.Uri;
            _keysRoot = root.AppendPath("v2").AppendPath("keys");
            _client = new RestClient(root.ToString());

            Statistics = new StatisticsModule(root, _client);
            Machine = new MachineModule(root);
        }

        /// <summary>
        ///     Sets the key to the provided value.
        ///     etcd will automatically creates directories as needed
        ///     * You can create hidden keys by prefixing key with '_'
        /// </summary>
        /// <param name="key">a hierarchical key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <param name="value">etcd only supports string values</param>
        /// <param name="prevExist">Used to compare and swap on existence</param>
        /// <param name="prevValue">Used to compare and swap on value</param>
        /// <param name="prevIndex">Used to compare and swap on index</param>
        /// <returns></returns>
        public EtcdResponse Set(string key, string value, int ttl = 0, bool? prevExist = null, string prevValue = null,
            int? prevIndex = null)
        {
            return makeKeyRequest(key, Method.PUT, req =>
            {
                req.AddParameter("value", value);
                if (ttl > 0)
                {
                    req.AddParameter("ttl", ttl);
                }

                if (prevExist.HasValue)
                {
                    var val = prevExist.Value ? "true" : "false";
                    req.AddParameter("prevExist", val);
                }

                if (prevValue != null)
                {
                    req.AddParameter("prevValue", prevValue);
                }

                if (prevIndex.HasValue)
                {
                    req.AddParameter("prevIndex", prevIndex.Value);
                }
            });
        }

        /// <summary>
        ///     Creates a dir
        /// </summary>
        /// <param name="key">the directory key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <param name="prevExist">Used to compare and swap on existence</param>
        /// <returns></returns>
        public EtcdResponse CreateDir(string key, int ttl = 0, bool prevExist = false)
        {
            return makeKeyRequest(key, Method.PUT, req =>
            {
                req.AddParameter("dir", "true");
                if (ttl > 0)
                {
                    req.AddParameter("ttl", ttl);
                }
                if (prevExist)
                {
                    req.AddParameter("prevExist", "true");
                }
            });
        }

        /// <summary>
        ///     Get the value of the key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="recursive">get recursively all the contents under a directory</param>
        /// <param name="sorted">if getting a directory, this will return the keys sorted by index</param>
        /// <param name="consistent">if you need the most up-to-date value, set this to true</param>
        /// <returns></returns>
        public EtcdResponse Get(string key, bool recursive = false, bool sorted = false, bool consistent = false)
        {
            return makeKeyRequest(key, Method.GET, req =>
            {
                req.AddParameter("recursive", recursive.ToString().ToLower());
                req.AddParameter("sorted", sorted.ToString().ToLower());
                req.AddParameter("consistent", sorted.ToString().ToLower());
            });
        }

        /// <summary>
        ///     Will create a queued key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>note the key will be 'key/index' in the return object</returns>
        public EtcdResponse Queue(string key, object value)
        {
            return makeKeyRequest(key, Method.POST, req => { req.AddParameter("value", value); });
        }


        /// <summary>
        ///     deletes a key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="prevValue">Compare and Delete on prevValue</param>
        /// <param name="prevIndex">Compare and Delete on prevIndex</param>
        /// <returns></returns>
        public EtcdResponse Delete(string key, string prevValue = null, int? prevIndex = null)
        {
            return makeKeyRequest(key, Method.DELETE, req =>
            {
                if (prevValue != null)
                {
                    req.AddParameter("prevValue", prevValue);
                }
                if (prevIndex.HasValue)
                {
                    req.AddParameter("prevIndex", prevIndex.Value);
                }
            });
        }

        /// <summary>
        ///     deletes a directory, must pass recursive if you want to delete non-empty dirs
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="recursive">sholud this delete all 'sub keys'</param>
        /// <returns></returns>
        public EtcdResponse DeleteDir(string key, bool recursive = false)
        {
            return makeKeyRequest(key, Method.DELETE, req =>
            {
                req.AddParameter("dir", "true");
                if (recursive) req.AddParameter("recursive", "true");
            });
        }

        /// <summary>
        ///     Sets up a watch on a keyspace and will call the callback when triggered
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="followUp">callback</param>
        /// <param name="recursive">watch subkeys?</param>
        /// <param name="timeout">How long will we watch?</param>
        /// <param name="waitIndex">Index to wait from</param>
        public void Watch(string key, Action<EtcdResponse> followUp, bool recursive = false, int? timeout = null, int? waitIndex = null)
        {
            var requestUrl = _keysRoot.AppendPath(key);
            var getRequest = new RestRequest(requestUrl, Method.GET);
            getRequest.AddParameter("wait", "true");

            if (recursive)
            {
                getRequest.AddParameter("recursive", "true");
            }

            if (timeout.HasValue)
            {
                getRequest.Timeout = timeout.Value * 1000;
            }

            if (waitIndex.HasValue)
            {
                getRequest.AddParameter("waitIndex", waitIndex);
            }

            _client.ExecuteAsync<EtcdResponse>(getRequest, r => followUp(processRestResponse(r)));
        }

        EtcdResponse makeKeyRequest(string key, Method method, Action<IRestRequest> action = null)
        {
            var requestUrl = _keysRoot.AppendPath(key);
            var request = new RestRequest(requestUrl, method);

            if (action != null) action(request);


            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<EtcdResponse>(request);

            if(checkForError(response)) throw constructException(response);
            
            var etcdResponse = processRestResponse(response);
            
            return etcdResponse;
        }


        static EtcdResponse processRestResponse(IRestResponse<EtcdResponse> response)
        {
            if (response == null) return null;

            var etcdResponse = response.Data;
            if (etcdResponse != null)
            {
                // While X-Raft-Index and X-Raft-Term have been noticed as missing (e.g., when Compare and Delete fails), X-Etcd-Index should always exist.
                etcdResponse.Headers.EtcdIndex = int.Parse(response.Headers.First(h => h.Name.Equals("X-Etcd-Index")).Value.ToString());

                var raftIndexHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("X-Raft-Index"));
                if (raftIndexHeader != null) etcdResponse.Headers.RaftIndex = int.Parse(raftIndexHeader.Value.ToString());

                var raftTermHeader = response.Headers.FirstOrDefault(h => h.Name.Equals("X-Raft-Term"));
                if (raftTermHeader != null) etcdResponse.Headers.RaftTerm = int.Parse(raftTermHeader.Value.ToString());
            }

            return etcdResponse;
        }


        static bool checkForError(IRestResponse<EtcdResponse> response)
        {
            return response.StatusCode == 0;
        }

        Exception constructException(IRestResponse<EtcdResponse> response)
        {
            var msg = new StringBuilder();
            msg.AppendFormat("Server: '{0}'", _client.BaseUrl);
            msg.AppendFormat("- Path: '{0}'", response.Request.Resource);
            msg.AppendFormat("- Message: '{0}'", response.ErrorMessage);

            return new EtceteraException(msg.ToString(), response.ErrorException);
        }

        public IEtcdStatisticsModule Statistics { get; private set; }

        public IEtcdMachineModule Machine { get; private set; }

        public X509CertificateCollection ClientCertificates
        {
            get
            {
                return _client.ClientCertificates;
            }
            set
            {
                _client.ClientCertificates = value;
            }
        }
    }
}
