namespace etcetera
{
    using System;
    using RestSharp;

    public interface IEtcdMembersModule
    {
        EtcdListMemberResponse List();
        EtcdAddMemberResponse Add(EtcdAddMemberRequest request);
        EtcdDeleteMemberResponse Delete(string id);
        EtcdChangePeersResponse ChangePeers(string id);
    }

    public class MembersModule : IEtcdMembersModule
    {
        readonly Uri _root;
        readonly IRestClient _client;

        public MembersModule(Uri root, IRestClient client)
        {
            _root = new UriBuilder(root.Scheme, root.Host, 7001, root.LocalPath).Uri;

            _client = client;
        }

        public EtcdListMemberResponse List()
        {
            return makeMemberRequest<EtcdListMemberResponse>();
        }

        public EtcdAddMemberResponse Add(EtcdAddMemberRequest request)
        {
            return makeMemberRequest<EtcdAddMemberResponse>(method:Method.POST,action: req =>
            {
                req.AddBody(request);
            });
        }

        public EtcdDeleteMemberResponse Delete(string id)
        {
            return makeMemberRequest<EtcdDeleteMemberResponse>(id, Method.DELETE);
        }

        public EtcdChangePeersResponse ChangePeers(string id)
        {
            return makeMemberRequest<EtcdChangePeersResponse>(id.ToString(), Method.PUT);
        }

        TResponse makeMemberRequest<TResponse>(string key = null, Method method=Method.GET, Action<IRestRequest> action = null) where TResponse : new()
        {
            var requestUrl = _root.AppendPath("v2").AppendPath("members");
            if (!string.IsNullOrEmpty(key)) requestUrl = requestUrl.AppendPath(key);

            var request = new RestRequest(requestUrl, method);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "");

            var response = _client.Execute<TResponse>(request);
            return response.Data;
        }
    }
}