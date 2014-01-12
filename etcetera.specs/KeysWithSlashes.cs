namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class KeysWithSlashes :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public KeysWithSlashes()
        {
            _response = Client.Set("/folder1/bill", "wassup");
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
            _response.Node.Key.ShouldEqual("/folder1/bill");
        }
    }
}