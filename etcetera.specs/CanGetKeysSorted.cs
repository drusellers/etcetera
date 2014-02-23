namespace etcetera.specs
{
    using System.Linq;
    using Should;
    using Xunit;

    public class CanGetKeysSorted :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeysSorted()
        {
            Client.Set(ADirectory + "/" +AKey, "wassup");
            Client.Set(ADirectory + "/" +AKey + "-2", "not-much");
            _getResponse = Client.Get(ADirectory, sorted:true);
        }

        [Fact]
        public void ActionIsGet()
        {
            _getResponse.Action.ShouldEqual("get");
        }

        [Fact]
        public void ValueIsNotPresent()
        {
            _getResponse.Node.Value.ShouldEqual(null);
        }

        [Fact]
        public void ReturnsChildrenSorted()
        {
            var a = _getResponse.Node.Nodes.First();
            var b = _getResponse.Node.Nodes.Last();
            a.CreatedIndex.ShouldBeLessThan(b.CreatedIndex);
        }

        [Fact]
        public void KeyIsSet()
        {
            _getResponse.Node.Key.ShouldEqual("/" + ADirectory);
        }

        [Fact]
        public void IsADir()
        {
            _getResponse.Node.Dir.ShouldBeTrue();
        }
    }
}