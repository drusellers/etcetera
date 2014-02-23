// ReSharper disable once CheckNamespace
namespace etcetera
{
    using System;
    using RestSharp;

    public class EtcdLockModule : IEtcdLockModule
    {
        readonly Uri _lockRoot;
        readonly IRestClient _client;

        public EtcdLockModule(Uri root, IRestClient client)
        {
            _lockRoot = root.AppendPath("mod").AppendPath("v2").AppendPath("lock");
            _client = client;
        }

        /// <summary>
        ///     Access the lock module of Etcd
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

        public string Release(string key, int index)
        {
            return makeLockRequest(key, Method.DELETE, req => { req.AddParameter("index", index); });
        }

        string makeLockRequest(string key, Method method, Action<IRestRequest> action)
        {
            var requestUrl = _lockRoot.AppendPath(key);
            var request = new RestRequest(requestUrl, method);

            action(request);

            var response = _client.Execute(request);
            return response.Content;
        }
    }
}