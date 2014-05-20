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
            _getResponse = Client.Get(AKey);
        }

        [Fact]
        public void EtcdIndex()
        {
            _getResponse.Headers.EtcdIndex.ShouldBeGreaterThan(1);
        }

        [Fact]
        public void RaftIndex()
        {
            _getResponse.Headers.RaftIndex.ShouldBeGreaterThan(22);
        }

        [Fact]
        public void KeyIsSet()
        {
            _getResponse.Headers.RaftTerm.ShouldEqual(0);
        }

    }
}