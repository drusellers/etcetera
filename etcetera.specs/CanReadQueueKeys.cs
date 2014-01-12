namespace etcetera.specs
{
    using System;
    using System.Linq;
    using Should;
    using Xunit;

    public class CanReadQueueKeys
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _id = Guid.NewGuid();

        public CanReadQueueKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _etcdClient.Queue(_id.ToString(), "wassup1");
            _etcdClient.Queue(_id.ToString(), "wassup2");

            _response = _etcdClient.Get(_id.ToString(), sorted:true);
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("get");
        }

        [Fact]
        public void NodeIsDir()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void NodesHas2Values()
        {
            _response.Node.Nodes.Count().ShouldEqual(2);
        }
    }
}