namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanCreateDirs
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();

        public CanCreateDirs()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.CreateDir(_key.ToString());
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Fact]
        public void NodeIsDirectory()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + _key);
        }

        [Fact]
        public void NoValue()
        {
            _response.Node.Value.ShouldBeNull();
        }
    }
}