namespace etcetera.specs
{
    using System.Threading;
    using Should;
    using Xunit;

    public class CanWatchKeysRecursively :
        EtcdBase
    {
        ManualResetEvent _wasHit;

        [Fact]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            Client.Set("bob/" + AKey, "wassup");

            Client.Watch("bob", resp =>
            {
                _wasHit.Set();
            });

            Client.Set("bob/" + AKey, "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }
    }
}