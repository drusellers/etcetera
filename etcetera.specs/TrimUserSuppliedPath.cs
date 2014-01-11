namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class TrimUserSuppliedPath
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;

        public TrimUserSuppliedPath()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/v2/keys"));
            _response = _etcdClient.Set("dru", "wassup");
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
            _response.Node.Key.ShouldEqual("/dru");
        }
    }
}