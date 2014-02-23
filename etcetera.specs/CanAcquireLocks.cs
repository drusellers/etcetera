using System;
using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanAcquireLocks : EtcdBase
    {
        [Fact]
        public void NegativeIsAnError()
        {
            var resp = Client.Lock.Lock(AKey, -10);
            resp.ShouldContain("202: The given TTL in POST form is not a number (Create)");
        }

        [Fact]
        public void GetsLocks()
        {
            var resp = Client.Lock.Lock(AKey, 10);
            Convert.ToInt32(resp).ShouldBeGreaterThan(0);
        }


        [Fact]
        public void CanExtendLocks()
        {
            var resp = Client.Lock.Lock(AKey, 10);
            Convert.ToInt32(resp).ShouldBeGreaterThan(0);

            var resp2 = Client.Lock.Lock(AKey, 10, index: Convert.ToInt32(resp));
            resp2.ShouldEqual("");
        }

        [Fact]
        public void CannotExtendNonExistentLocks()
        {
            var resp2 = Client.Lock.Lock(AKey, 10, index: 5);
            resp2.ShouldStartWith(string.Format("100: Key not found (/_etcd/mod/lock/{0})", AKey));
        }

        [Fact]
        public void CanReleaseALock()
        {
            var resp = Client.Lock.Lock(AKey, 10);
            var resp2 = Client.Lock.Release(AKey, Convert.ToInt32(resp));
            resp2.ShouldEqual("");
        }

    }
}