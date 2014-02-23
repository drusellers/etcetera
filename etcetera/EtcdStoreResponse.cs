namespace etcetera
{
    public class EtcdStoreResponse
    {
        public int CompareAndSwapFail { get; set; }
        public int CompareAndSwapSuccess { get; set; }
        public int CreateFail { get; set; }
        public int CreateSuccess { get; set; }
        public int DeleteFail { get; set; }
        public int DeleteSuccess { get; set; }
        public int ExpireCount { get; set; }
        public int GetsFail { get; set; }
        public int GetsSuccess { get; set; }
        public int SetsFail { get; set; }
        public int SetsSuccess { get; set; }
        public int UpdateFail { get; set; }
        public int UpdateSuccess { get; set; }
        public int Watchers { get; set; }
    }
}