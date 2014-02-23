namespace etcetera
{
    using System.Collections.Generic;

    public class EtcdLeaderStatsResponse
    {
        public Dictionary<string, EtcdNode> Followers { get; set; }
        public string Leader { get; set; }


        public class EtcdNode
        {
            public EtcdNodeCounts Counts { get; set; }
            public EtcdNodeLatency Latency { get; set; }
        }

        public class EtcdNodeCounts
        {
            public int Fail { get; set; }
            public int Success { get; set; }
        }

        public class EtcdNodeLatency
        {
            public int Average { get; set; }
            public int Current { get; set; }
            public int Maximum { get; set; }
            public int Minimum { get; set; }
            public int StandardDeviation { get; set; }
        }
    }

}