namespace etcetera
{
    using System;
    using RestSharp;

    public interface IEtcdStatisticsModule
    {
        EtcdStoreResponse Store();
        EtcdLeaderStatsResponse Leader();
        EtcdSelfStatistics Self();
    }

    public class StatisticsModule : IEtcdStatisticsModule
    {
        readonly Uri _root;
        readonly IRestClient _client;

        public StatisticsModule(Uri root, IRestClient restClient)
        {
            _root = root;
            _client = restClient;
        }

        public EtcdStoreResponse Store()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("stats").AppendPath("store");
            var request = new RestRequest(requestUrl, Method.GET);

            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<EtcdStoreResponse>(request);
            return response.Data;
        }

        public EtcdLeaderStatsResponse Leader()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("stats").AppendPath("leader");
            var request = new RestRequest(requestUrl, Method.GET);

            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<EtcdLeaderStatsResponse>(request);
            return response.Data;
        }

        public EtcdSelfStatistics Self()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("stats").AppendPath("self");
            var request = new RestRequest(requestUrl, Method.GET);

            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<EtcdSelfStatistics>(request);
            return response.Data;
        }
    }
}