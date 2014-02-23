// ReSharper disable once CheckNamespace
namespace etcetera
{
    public interface IEtcdLockModule
    {
        /// <summary>
        ///     Access the lock module of Etcd
        /// </summary>
        /// <param name="key">The key to acquire the lock on</param>
        /// <param name="ttl">The time to live in seconds of the lock</param>
        /// <param name="index">You can renew a lock by providing the previous index</param>
        /// <returns></returns>
        string Lock(string key, int ttl, int? index = null);

        string Release(string key, int index);
    }
}