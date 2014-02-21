using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanGetStoreStats : EtcdBase
    {
        [Fact]
        public void CanSeeStats()
        {
            var resp = Client.StoreStats();
            resp.ShouldNotBeNull();
        }
    }
}