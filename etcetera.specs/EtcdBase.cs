namespace etcetera.specs
{
    using System;

    public abstract class EtcdBase
    {
        public string AKey = Guid.NewGuid().ToString();
        public EtcdClient Client { get; set; }

        public EtcdBase()
        {
            Client = new EtcdClient(new Uri("http://192.168.101.1:4001/"));
        }
    }
}