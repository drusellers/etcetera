namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class KeysWithSlashes
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;

        public KeysWithSlashes()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.Set("/folder1/bill", "wassup");
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/folder1/bill");
        }
    }
}