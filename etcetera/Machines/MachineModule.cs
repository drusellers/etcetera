namespace etcetera
{
    using System;
    using RestSharp;

    public interface IEtcdMachineModule
    {
        EtcdMachineResponse Get(string machineName);
        EtcdListMachineResponse List();        
    }

    public class MachineModule : IEtcdMachineModule
    {
        readonly Uri _root;
        readonly IRestClient _client;

        public MachineModule(Uri root)
        {
            _root = new UriBuilder(root.Scheme, root.Host, 7001, root.LocalPath).Uri;

            _client = new RestClient(_root.ToString());
        }

        public EtcdMachineResponse Get(string machineName)
        {
            return makeMachineRequest<EtcdMachineResponse>(machineName);
        }

        public EtcdListMachineResponse List()
        {
            return makeMachineRequest<EtcdListMachineResponse>();
        }
         
        TResponse makeMachineRequest<TResponse>(string key = null) where TResponse:new()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("admin").AppendPath("machines");
            if (!string.IsNullOrEmpty(key)) requestUrl = requestUrl.AppendPath(key); 

            var request = new RestRequest(requestUrl, Method.GET);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "");

            //needed due to issue 469 - https://github.com/coreos/etcd/issues/469
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var response = _client.Execute<TResponse>(request);
            return response.Data;
        }
    }
}