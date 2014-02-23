namespace etcetera
{
    public class EtcdResponse
    {
        public string Action { get; set; }
        public Node Node { get; set; }


        //ttl error
        public int? ErrorCode { get; set; }
        public string Cause { get; set; }
        public int? Index { get; set; }
        public string Message { get; set; }
        public Node PrevNode { get; set; }
    }
}