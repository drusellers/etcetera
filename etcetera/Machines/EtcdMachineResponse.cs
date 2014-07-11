namespace etcetera
{
    public class EtcdMachineResponse
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string ClientUrl { get; set; }
        public string PeerUrl { get; set; }
    }
}