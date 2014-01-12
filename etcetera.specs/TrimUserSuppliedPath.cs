namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class TrimUserSuppliedPath :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public TrimUserSuppliedPath()
        {
            _response = Client.Set(AKey, "wassup");
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _response.Node.Value.ShouldEqual("wassup");
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/"+AKey);
        }
    }
}