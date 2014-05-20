namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class ProvidesHeaders :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public ProvidesHeaders()
        {
            Client.Set(AKey, "wassup");
            Client.Set(AKey, "wassup2");
            _getResponse = Client.Get(AKey);
        }

        [Fact]
        public void EtcdIndex()
        {
            //TODO: make test more robust
            _getResponse.Headers.EtcdIndex.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void RaftIndex()
        {
            _getResponse.Headers.RaftIndex.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void KeyIsSet()
        {
            // From the documentation: "is an integer that will increase whenever an etcd master election happens in the cluster."
            // Therefore, for any cluster, it should be at least 1.
            _getResponse.Headers.RaftTerm.ShouldBeGreaterThan(0);
        }

    }
}