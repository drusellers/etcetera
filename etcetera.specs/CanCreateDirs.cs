namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanCreateDirs :
        EtcdBase
    {
        readonly EtcdResponse _response;

        public CanCreateDirs()
        {
            _response = Client.CreateDir(AKey);
        }

        [Fact]
        public void ActionIsSet()
        {
            _response.Action.ShouldEqual("set");
        }

        [Fact]
        public void NodeIsDirectory()
        {
            _response.Node.Dir.ShouldBeTrue();
        }

        [Fact]
        public void KeyIsSet()
        {
            _response.Node.Key.ShouldEqual("/" + AKey);
        }

        [Fact]
        public void NoValue()
        {
            _response.Node.Value.ShouldBeNull();
        }
    }
}