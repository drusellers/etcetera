namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanUpdateDirsWithTtl :
        EtcdBase
    {
        EtcdResponse _response;
        int _ttl = 30;
        DateTime _now;

        public CanUpdateDirsWithTtl()
        {
            Client.CreateDir(AKey);
            _response = Client.CreateDir(AKey, _ttl, true);
            _now = DateTime.Now;
        }

        [Fact]
        public void ActionIsUpdate()
        {
            _response.Action.ShouldEqual("update");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey);
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

        [Fact]
        public void PrevKeyIsSet()
        {
            _response.PrevNode.Key.ShouldEqual("/" + AKey);
        }

        [Fact]
        public void PrevTtlIsNotSet()
        {
            _response.PrevNode.Ttl.ShouldBeNull();
        }

        [Fact]
        public void PrevIsDir()
        {
            _response.PrevNode.Dir.ShouldBeTrue();
        }

        [Fact]
        public void PrevExpirationIsNotSet()
        {
            _response.PrevNode.Expiration.ShouldBeNull();
        }
    }
}
