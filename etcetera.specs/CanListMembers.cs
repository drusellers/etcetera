namespace etcetera.specs
{
    using Should;
    using Xunit;

    public class CanListMembers : EtcdBase
    {
        [Fact]
        public void CanGetListOfMembers()
        {
            var resp = Client.Members.List();
            resp.Members.ShouldNotBeEmpty();
        }
    }
}