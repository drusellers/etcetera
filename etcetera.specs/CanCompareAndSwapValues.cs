using Should;
using Xunit;

namespace etcetera.specs
{
    public class CanCompareAndSwapValues : EtcdBase
    {
        [Fact]
        public void ErrorsIfPreviousKeyIsPresent()
        {
            Client.Set(AKey, "one");
            var rep2 = Client.Set(AKey, "three", prevExist:false);


            rep2.ErrorCode.ShouldEqual(105);
            rep2.Cause.ShouldEqual("/"+AKey);
            rep2.Index.ShouldBeGreaterThan(0);
            rep2.Message.ShouldEqual("Key already exists");
        }

        [Fact]
        public void ErrorsIfPreviousKeyIsNotPresent()
        {
            var rep = Client.Set(AKey, "three", prevExist: true);


            rep.ErrorCode.ShouldEqual(100);
            rep.Cause.ShouldEqual("/" + AKey);
            rep.Index.ShouldBeGreaterThan(0);
            rep.Message.ShouldEqual("Key not found");
        }

        [Fact]
        public void SupportsPreviousValue()
        {
            var one = "one";
            var two = "two";

            Client.Set(AKey, one);
            var rep2 = Client.Set(AKey, "three", prevValue: two);

            rep2.ErrorCode.ShouldEqual(101);
            rep2.Cause.ShouldStartWith(string.Format("[{0} != {1}]", two, one));
            rep2.Index.ShouldBeGreaterThan(0);
            rep2.Message.ShouldEqual("Compare failed");
        }

        [Fact]
        public void SupportsPreviousIndex()
        {
            var one = "one";

            var rep1 = Client.Set(AKey, one);
            var rep2 = Client.Set(AKey, "three", prevIndex: rep1.Node.CreatedIndex+1);

            rep2.ErrorCode.ShouldEqual(101);
            rep2.Cause.ShouldEqual(string.Format("[{0} != {1}]", rep1.Node.CreatedIndex + 1, rep1.Node.CreatedIndex));
            rep2.Index.ShouldBeGreaterThan(0);
            rep2.Message.ShouldEqual("Compare failed");
        }

        [Fact]
        public void ReturnsCompareAndSwapData()
        {
            var one = "one";

            Client.Set(AKey, one);
            var rep2 = Client.Set(AKey, "three", prevValue: one);

            rep2.Action.ShouldEqual("compareAndSwap");
            rep2.Node.Key.ShouldEqual("/"+AKey);
            rep2.Node.Value.ShouldEqual("three");

            rep2.PrevNode.Value.ShouldEqual(one);
        }
    }
}