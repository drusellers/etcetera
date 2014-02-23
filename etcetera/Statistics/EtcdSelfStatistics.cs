namespace etcetera
{
    public class EtcdSelfStatistics
    {
        public EtcdLeaderInfo LeaderInfo { get; set; }
        public string Name { get; set; }
        public string RecvAppendRequentCnt { get; set; }
        public string SendAppendRequentCnt { get; set; }
        public string RecvBandwidthRate { get; set; }
        public string SendBandwidthRate { get; set; }
        public string RecvPkgRate { get; set; }
        public string SendPkgRate { get; set; }
        public string SendAppendRequestCnt { get; set; }
        public string StartTime { get; set; }
        public string State { get; set; }

        public class EtcdLeaderInfo
        {
            public string Leader { get; set; }
            public string Uptime { get; set; }
        }
    }
}