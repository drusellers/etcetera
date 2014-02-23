namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanGetKeysRecursively :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeysRecursively()
        {
            Client.Set(ADirectory + "/" +AKey, "wassup");
            Client.Set(ADirectory + "/" +AKey + "-2", "not-much");
            _getResponse = Client.Get(ADirectory, recursive:true);
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
        public void ReturnsChildren()
        {
            _getResponse.Node.Nodes.Count.ShouldEqual(2);
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