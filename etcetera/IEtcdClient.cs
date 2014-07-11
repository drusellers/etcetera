namespace etcetera
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    public interface IEtcdClient
    {
        /// <summary>
        ///     Sets the key to the provided value.
        ///     etcd will automatically creates directories as needed
        ///     * You can create hidden keys by prefixing key with '_'
        /// </summary>
        /// <param name="key">a hierarchical key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <param name="value">etcd only supports string values</param>
        /// <param name="prevExist">Used to compare and swap on existence</param>
        /// <param name="prevValue">Used to compare and swap on value</param>
        /// <param name="prevIndex">Used to compare and swap on index</param>
        /// <returns></returns>
        EtcdResponse Set(string key, string value, int ttl = 0, bool? prevExist = null, string prevValue = null,
            int? prevIndex = null);


        /// <summary>
        ///     Creates a dir
        /// </summary>
        /// <param name="key">the directory key</param>
        /// <param name="ttl">time to live in seconds</param>
        /// <param name="prevExist">Used to compare and swap on existence</param>
        /// <returns></returns>
        EtcdResponse CreateDir(string key, int ttl = 0, bool prevExist = false);


        /// <summary>
        ///     Get the value of the key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="recursive">get recursively all the contents under a directory</param>
        /// <param name="sorted">if getting a directory, this will return the keys sorted by index</param>
        /// <param name="consistent">if you need the most up-to-date value, set this to true</param>
        /// <returns></returns>
        EtcdResponse Get(string key, bool recursive = false, bool sorted = false, bool consistent = false);

        /// <summary>
        ///     Will create a queued key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>note the key will be 'key/index' in the return object</returns>
        EtcdResponse Queue(string key, object value);

        /// <summary>
        ///     deletes a key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="prevValue">Compare and Delete on prevValue</param>
        /// <param name="prevIndex">Compare and Delete on prevIndex</param>
        /// <returns></returns>
        EtcdResponse Delete(string key, string prevValue = null, int? prevIndex = null);


        /// <summary>
        ///     deletes a directory, must pass recursive if you want to delete non-empty dirs
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="recursive">sholud this delete all 'sub keys'</param>
        /// <returns></returns>
        EtcdResponse DeleteDir(string key, bool recursive = false);

        /// <summary>
        ///     Sets up a watch on a keyspace and will call the callback when triggered
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="followUp">callback</param>
        /// <param name="recursive">watch subkeys?</param>
        /// <param name="timeout">How long will we watch?</param>
        /// <param name="waitIndex">Index to wait from</param>
        void Watch(string key, Action<EtcdResponse> followUp, bool recursive = false, int? timeout = null, int? waitIndex = null);

        IEtcdStatisticsModule Statistics { get; }

        IEtcdMachineModule Machine { get; }

        X509CertificateCollection ClientCertificates { get; set; }
    }
}