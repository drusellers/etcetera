namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanGetKeys
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        EtcdResponse _getResponse;

        public CanGetKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.Set(_key.ToString(), "wassup");
            _getResponse = _etcdClient.Get(_key.ToString());
        }

        [Fact]
        public void ActionIsSet()
        {
            _getResponse.Action.ShouldEqual("get");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _getResponse.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _getResponse.Node.Key.ShouldEqual("/" + _key);
        }
    }
}