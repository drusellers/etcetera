using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanGetStoreStats : EtcdBase
    {
        [Fact]
        public void CanSeeStats()
        {
            var resp = Client.Statistics.Store();
            resp.ShouldNotBeNull();
        }
    }
}