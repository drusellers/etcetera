namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanDeleteDirsWithStuff
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        EtcdResponse _deleteResponse;

        public CanDeleteDirsWithStuff()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.Set(_key+"/bob", "hi");
            _deleteResponse = _etcdClient.DeleteDir(_key.ToString(), true);
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
        public void IsDir()
        {
            _deleteResponse.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + _key);
        }
    }
}