namespace etcetera.specs
{
    using System;
    using System.Threading;
    using Should;
    using Xunit;

    public class CanWatchKeysRecursively
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        ManualResetEvent _wasHit;


        public CanWatchKeysRecursively()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));

        }

        [Fact]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            _response = _etcdClient.Set("bob/"+_key, "wassup");

            _etcdClient.Watch("bob", resp =>
            {
                _wasHit.Set();
            });

            _etcdClient.Set("bob/"+_key, "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }


    }
}