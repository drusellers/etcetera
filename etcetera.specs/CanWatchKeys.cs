namespace etcetera.specs
{
    using System.Threading;
    using Should;
    using Xunit;

    public class CanWatchKeys :
        EtcdBase
    {
        ManualResetEvent _wasHit;

        [Fact]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            Client.Set(AKey, "wassup");
            
            Client.Watch(AKey, resp =>
            {
                _wasHit.Set();
            });

            Client.Set(AKey, "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }   
    }
}