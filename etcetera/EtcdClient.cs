using System.Net;
using System.Threading;

namespace etcetera
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestSharp;

    public class EtcdClient
    {
        readonly IRestClient _client;
        readonly Uri _root;
        readonly Uri _keysRoot;
        readonly Uri _lockRoot;

        public EtcdClient(Uri etcdLocation)
        {
            var uriBuilder = new UriBuilder(etcdLocation)
            {
               Path = ""
            };
            _root = uriBuilder.Uri;
            _keysRoot = _root.AppendPath("v2").AppendPath("keys");
            _lockRoot = _root.AppendPath("mod").AppendPath("v2").AppendPath("lock");
            _client = new RestClient(_root.ToString());
        }

        /// <summary>
        /// Sets the key to the provided value. 
        /// etcd will automatically creates directories as needed
        /// * You can create hidden keys by prefixing key with '_'
        /// </summary>
        /// <param name="key">a hierarchical key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <param name="value">etcd only supports string values</param>
        /// <param name="prevExist">Used to compare and swap on existance</param>
        /// <param name="prevValue">Used to compare and swap on value</param>
        /// <param name="prevIndex">Used to compare and swap on index</param>
        /// <returns></returns>
        public EtcdResponse Set(string key, string value, int ttl = 0, bool? prevExist = null, string prevValue=null, int? prevIndex=null)
        {
            return makeKeyRequest(key, Method.PUT, req =>
            {
                //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
                req.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                req.AddParameter("value", value);
                if (ttl > 0)
                {
                    req.AddParameter("ttl", ttl);
                }
                if (prevExist.HasValue)
                {
                    req.AddParameter("prevExist", prevExist.Value.ToString().ToLower());
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
        /// Creates a dir
        /// </summary>
        /// <param name="key">the directory key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <returns></returns>
        public EtcdResponse CreateDir(string key, int ttl = 0)
        {
            return makeKeyRequest(key, Method.PUT, req =>
            {
                req.AddParameter("dir", "true");
                if (ttl > 0)
                {
                    req.AddParameter("ttl", ttl);
                }
            });
        }

        /// <summary>
        /// Get the value of the key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="recursive">get recursively all the contents under a directory</param>
        /// <param name="sorted">if getting a directory, this will return the keys sorted by index</param>
        /// <returns></returns>
        public EtcdResponse Get(string key, bool recursive = false, bool sorted = false)
        {
            return makeKeyRequest(key, Method.GET, req =>
            {
                //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
                req.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

                if (recursive)
                {
                    req.AddParameter("recursive", true);
                }

                if (sorted)
                {
                    req.AddParameter("sorted", true);
                }
            });
        }

        /// <summary>
        /// Will create a queued key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>note the key will be 'key/index' in the return object</returns>
        public EtcdResponse Queue(string key, object value)
        {
            return makeKeyRequest(key, Method.POST, req =>
            {
                req.AddParameter("value", value);
            });
        }


        /// <summary>
        /// deletes a key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="prevValue">Compare and Delete on prevValue</param>
        /// <param name="prevIndex">Compare and Delete on prevIndex</param>
        /// <returns></returns>
        public EtcdResponse Delete(string key, string prevValue = null, int? prevIndex = null)
        {
            return makeKeyRequest(key, Method.DELETE, req =>
            {
                //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
                req.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

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
        /// deletes a directory, must pass recursive if you want to delete non-empty dirs
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
        /// Sets up a watch on a keyspace and will call the callback when triggered
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="followUp">callback</param>
        /// <param name="recursive">watch subkeys?</param>
        /// <param name="timeout">How long will we watch?</param>
        public void Watch(string key, Action<EtcdResponse> followUp, bool recursive = false, int? timeout = null)
        {
            var requestUrl = _keysRoot.AppendPath(key);
            var getRequest = new RestRequest(requestUrl, Method.GET);
            getRequest.AddParameter("wait", "true");
            
            if (recursive)
            {
                getRequest.AddParameter("recursive", recursive);
            }

            if (timeout.HasValue)
            {
                getRequest.Timeout = timeout.Value;
            }

            _client.ExecuteTaskAsync<EtcdResponse>(getRequest)
                .ContinueWith(t => followUp(t.Result.Data));
        }

        EtcdResponse makeKeyRequest(string key, Method method, Action<IRestRequest> action = null)
        {
            var requestUrl = _keysRoot.AppendPath(key);
            var request = new RestRequest(requestUrl, method);
            
            
            if(action != null) action(request);

            var response = _client.Execute<EtcdResponse>(request);
            return response.Data;
        }


        /// <summary>
        /// Access the lock module of Etcd
        /// </summary>
        /// <param name="key">The key to acquire the lock on</param>
        /// <param name="ttl">The time to live in seconds of the lock</param>
        /// <param name="index">You can renew a lock by providing the previous index</param>
        /// <returns></returns>
        public string Lock(string key, int ttl, int? index = null)
        {
            var method = index.HasValue ? Method.PUT : Method.POST;
            return makeLockRequest(key, method, req =>
            {
                req.AddParameter("ttl", ttl);

                if (index.HasValue)
                {
                    req.AddParameter("index", index.Value);
                }
            });
        }

        public string ReleaseLock(string key, int index)
        {
            return makeLockRequest(key, Method.DELETE, req =>
            {
                req.AddParameter("index", index);
            });
        }

        string makeLockRequest(string key, Method method, Action<IRestRequest> action)
        {
            var requestUrl = _lockRoot.AppendPath(key);
            var request = new RestRequest(requestUrl, method);

            action(request);

            var response = _client.Execute(request);
            return response.Content;
        }

        public EtcdStoreResponse StoreStats()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("stats").AppendPath("store");
            var request = new RestRequest(requestUrl, Method.GET);
            
            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<EtcdStoreResponse>(request);
            return response.Data;
        }

        //TODO: stats /v2/stats/leader
        //TODO: stats /v2/stats/self
    }

    public class EtcdStoreResponse
    {
        public int CompareAndSwapFail { get; set; }
        public int CompareAndSwapSuccess { get; set; }
        public int CreateFail { get; set; }
        public int CreateSuccess { get; set; }
        public int DeleteFail { get; set; }
        public int DeleteSuccess { get; set; }
        public int ExpireCount { get; set; }
        public int GetsFail { get; set; }
        public int GetsSuccess { get; set; }
        public int SetsFail { get; set; }
        public int SetsSuccess { get; set; }
        public int UpdateFail { get; set; }
        public int UpdateSuccess { get; set; }
        public int Watchers { get; set; }
    }

    public class EtcdResponse
    {
        public string Action { get; set; }
        public Node Node { get; set; }


        //ttl error
        public int? ErrorCode { get; set; }
        public string Cause { get; set; }
        public int? Index { get; set; }
        public string Message { get; set; }
        public Node PrevNode { get; set; }
    }

    public static class EtcResponseHelpers
    {
        public static int EtcIndex(this IRestResponse response)
        {
            return (int)response.Headers.First(x=>x.Name == "X-Etcd-Index").Value;
        }

        public static int EtcRaftIndex(this IRestResponse response)
        {
            return (int)response.Headers.First(x=>x.Name == "X-Raft-Index").Value;
        }

        public static int EtcRaftTerm(this IRestResponse response)
        {
            return (int)response.Headers.First(x => x.Name == "X-Raft-Term").Value;
        }
    }

    public class Node
    {
        public int CreatedIndex { get; set; }
        public string Key { get; set; }
        public int ModifiedIndex { get; set; }
        public string Value { get; set; }
        public int? Ttl { get; set; }
        public DateTime? Expiration { get; set; }
        public List<Node> Nodes { get; set; }
        public bool Dir { get; set; }
    }

    public static class UriHelpers
    {
        public static Uri AppendPath(this Uri uri, string path)
        {
            var path1 = uri.AbsolutePath.TrimEnd(new []
            {
                '/'
            }) + "/" + path;
            return new UriBuilder(uri.Scheme, uri.Host, uri.Port, path1, uri.Query).Uri;
        }
    }
}
