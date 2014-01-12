namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanSetDirsWithTtl
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        int _ttl = 30;
        DateTime _now;
        public CanSetDirsWithTtl()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _response = _etcdClient.CreateDir(_key.ToString(), _ttl);
            _now = DateTime.Now;
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + _key);
        }

        [Fact]
        public void TtlIsSet()
        {
            _response.Node.Ttl.ShouldEqual(_ttl);
        }

        [Fact]
        public void IsDir()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void ExpirationIsSet()
        {
            _response.Node.Expiration.ShouldBeGreaterThan(_now.AddSeconds(_ttl).AddSeconds(-1));
            _response.Node.Expiration.ShouldBeLessThan(_now.AddSeconds(_ttl).AddSeconds(1));
        }
    }
}