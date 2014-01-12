namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanDeleteDirs :
        EtcdBase
    {
        readonly EtcdResponse _deleteResponse;

        public CanDeleteDirs()
        {
            Client.CreateDir(AKey);
            _deleteResponse = Client.DeleteDir(AKey);
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
        public void IsDir()
        {
            _deleteResponse.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void KeyIsSet()
        {
            _deleteResponse.Node.Key.ShouldEqual("/" + AKey);
        }
    }
}