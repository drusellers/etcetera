using System.Text;

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
            return makeStatsRequest<EtcdStoreResponse>("store", Method.GET);
        }

        public EtcdLeaderStatsResponse Leader()
        {
            return makeStatsRequest<EtcdLeaderStatsResponse>("leader", Method.GET);
        }

        public EtcdSelfStatistics Self()
        {
            return makeStatsRequest<EtcdSelfStatistics>("self", Method.GET);
        }

        TResponse makeStatsRequest<TResponse>(string key, Method verb) where TResponse:new()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("stats").AppendPath(key);
            var request = new RestRequest(requestUrl, verb);

            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<TResponse>(request);

            if (checkForError(response)) throw constructException(response);

            return response.Data;
        }

        static bool checkForError<TResponse>(IRestResponse<TResponse> response)
        {
            return response.StatusCode == 0;
        }

        Exception constructException<TResponse>(IRestResponse<TResponse> response)
        {
            var msg = new StringBuilder();
            msg.AppendFormat("Server: '{0}'", _client.BaseUrl);
            msg.AppendFormat("- Path: '{0}'", response.Request.Resource);
            msg.AppendFormat("- Message: '{0}'", response.ErrorMessage);

            return new EtceteraException(msg.ToString(), response.ErrorException);
        }
    }
}