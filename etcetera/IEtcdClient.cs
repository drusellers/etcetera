namespace etcetera
{
    public interface IEtcdClient
    {
        IEtcdStatisticsModule Statistics { get; }
        IEtcdLockModule Lock { get; }
    }
}