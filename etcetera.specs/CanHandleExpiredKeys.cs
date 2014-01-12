namespace etcetera.specs
{
    using System;
    using System.Threading;
    using Should;
    using Xunit;

    public class CanHandleExpiredKeys
    {
        readonly EtcdClient _etcdClient;
        Guid _key = Guid.NewGuid();
        const int _ttl = 1;

        public CanHandleExpiredKeys()
        {
            _etcdClient = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
            _etcdClient.Set(_key.ToString(), "wassup", _ttl);
        }

        [Fact]
        public void ActionIsSet()
        {
            Thread.Sleep(3000);
            var response = _etcdClient.Get(_key.ToString());

            response.ErrorCode.ShouldEqual(100);
        }


    }
}