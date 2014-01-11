namespace etcetera.specs
{
    using System;
    using System.Threading;
    using Should;
    using Xunit;

    public class CanWatchKeys
    {
        EtcdClient _etcdClient;
        EtcdResponse _response;
        Guid _key = Guid.NewGuid();
        ManualResetEvent _wasHit;


        public CanWatchKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            
        }

        [Fact]
        public void ActionIsSet()
        {
            _wasHit = new ManualResetEvent(false);
            _response = _etcdClient.Set(_key.ToString(), "wassup");
            
            _etcdClient.Watch(_key.ToString(), resp =>
            {
                _wasHit.Set();
            });

            _etcdClient.Set(_key.ToString(), "nope");
            _wasHit.WaitOne(1000).ShouldBeTrue();
        }

        
    }
}