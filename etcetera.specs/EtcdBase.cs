namespace etcetera.specs
{
    using System;

    public abstract class EtcdBase
    {
        public string AKey = Guid.NewGuid().ToString();
        public EtcdClient Client { get; set; }

        protected EtcdBase()
        {
            Client = new EtcdClient(new Uri("http://172.16.73.1:4001/"));
        }
    }
}