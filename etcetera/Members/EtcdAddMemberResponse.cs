namespace etcetera
{
    using System.Collections.Generic;

    public class EtcdAddMemberResponse
    {
        public string Id { get; set; }
        public List<string> PeerUrls { get; set; }
    }
}