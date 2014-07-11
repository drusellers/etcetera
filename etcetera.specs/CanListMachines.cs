using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanListMachines : EtcdBase
    {
        [Fact]
        public void CanGetAListOfMachines()
        {
            var resp = Client.Machine.List();
            resp.ShouldNotBeEmpty();
        }
    }
}