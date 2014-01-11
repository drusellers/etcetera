namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanQueueKeys
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _id = Guid.NewGuid();

        public CanQueueKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.Queue(_id.ToString(), "wassup");
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("create");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + _id + "/" + _response.Node.CreatedIndex);
        }

        [Fact]
        public void TtlIsNotSet()
        {
            _response.Node.Ttl.HasValue.ShouldBeFalse();
        }
    }
}