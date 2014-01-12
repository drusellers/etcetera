namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanQueueKeys :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public CanQueueKeys()
        {
            _response = Client.Queue(AKey, "wassup");
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("create");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey + "/" + _response.Node.CreatedIndex);
        }

        [Fact]
        public void TtlIsNotSet()
        {
            _response.Node.Ttl.HasValue.ShouldBeFalse();
        }
    }
}