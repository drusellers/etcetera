namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanDeleteKeys :
        EtcdBase
    {
        readonly EtcdResponse _deleteResponse;

        public CanDeleteKeys()
        {
            Client.Set(AKey, "wassup");
            _deleteResponse = Client.Delete(AKey);
        }

        [Fact]
        public void ActionIsSet()
        {
            _deleteResponse.Action.ShouldEqual("delete");
        }

        [Fact]
        public void ValueIsWassup()
        {
            _deleteResponse.Node.Value.ShouldBeNull();
        }

        [Fact]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + AKey);
        }
    }
}