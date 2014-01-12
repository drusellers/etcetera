namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanGetKeys :
        EtcdBase
    {
        readonly EtcdResponse _getResponse;

        public CanGetKeys()
        {
            Client.Set(AKey, "wassup");
            _getResponse = Client.Get(AKey);
        }

        [Fact]
        public void ActionIsSet()
        {
            _getResponse.Action.ShouldEqual("get");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _getResponse.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _getResponse.Node.Key.ShouldEqual("/" + AKey);
        }

        [Fact]
        public void IsNotADir()
        {
            _getResponse.Node.Dir.ShouldBeFalse();
        }
    }
}