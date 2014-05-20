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
            _getResponse.Headers.EtcdIndex.ShouldBeGreaterThan(-1);
        }

        [Fact]
        public void RaftIndex()
        {
            _getResponse.Headers.RaftIndex.ShouldBeGreaterThan(-1);
        }

        [Fact]
        public void KeyIsSet()
        {
            _getResponse.Headers.RaftTerm.ShouldEqual(0);
        }

    }
}