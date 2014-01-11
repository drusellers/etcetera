namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanDeleteKeys
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        EtcdResponse _deleteResponse;

        public CanDeleteKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.Set(_key.ToString(), "wassup");
            _deleteResponse = _etcdClient.Delete(_key.ToString());
        }

        [Fact]
        public void ActionIsSet()
        {
            _deleteResponse.Action.ShouldEqual("delete");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _deleteResponse.Node.Value.ShouldBeNull();
        }

        [Fact]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + _key);
        }
    }
}