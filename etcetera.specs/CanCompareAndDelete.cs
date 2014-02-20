using System.Runtime.Remoting.Messaging;
using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanCompareAndDelete : EtcdBase
    {
        [Fact]
        public void SupportPrevValue()
        {
            var one = "one";
            var two = "two";
            
            var rep1 = Client.Set(AKey, one);
            var rep2 = Client.Delete(AKey, prevValue:two);

            rep2.ErrorCode.ShouldEqual(101);
            rep2.Message.ShouldEqual("Compare failed");
            rep2.Cause.ShouldEqual(string.Format("[{0} != {1}] [0 != {2}]", two, one, rep1.Node.CreatedIndex));
        }

        [Fact]
        public void SupportPrevIndex()
        {
            var one = "one";
            var two = "two";

            var rep1 = Client.Set(AKey, one);
            var rep2 = Client.Delete(AKey, prevIndex: rep1.Node.CreatedIndex + 1);

            rep2.ErrorCode.ShouldEqual(101);
            rep2.Message.ShouldEqual("Compare failed");
            rep2.Cause.ShouldEqual(string.Format("[ != {0}] [{1} != {2}]", one, rep1.Node.CreatedIndex + 1, rep1.Node.CreatedIndex));
        }

        [Fact]
        public void ReturnsCompareAndDeleteData()
        {
            var one = "one";
            var two = "two";

            var rep1 = Client.Set(AKey, one);
            var rep2 = Client.Delete(AKey, prevIndex: rep1.Node.CreatedIndex);


            rep2.Action.ShouldEqual("compareAndDelete");
            rep2.ErrorCode.ShouldEqual(null);
            rep2.Message.ShouldEqual(null);


            rep2.Node.Key.ShouldEqual("/" + AKey);
            rep2.Node.Value.ShouldEqual(null);

            rep2.PrevNode.Value.ShouldEqual(one);
        }
    }
}