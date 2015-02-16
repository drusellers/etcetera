namespace etcetera
{
    using System.Collections.Generic;

    public class EtcdMember
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PeerUrls { get; set; }
        public List<string> ClientUrls { get; set; } 
    }
}