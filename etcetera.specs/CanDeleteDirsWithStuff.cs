namespace etcetera.specs
{
    using System;
    using Should;
    using Xunit;

    public class CanDeleteDirsWithStuff :
        EtcdBase
    {
        EtcdResponse _deleteResponse;

        public CanDeleteDirsWithStuff()
        {
            Client.Set(AKey +"/bob", "hi");
            _deleteResponse = Client.DeleteDir(AKey, true);
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