using System;
using Newtonsoft.Json;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 单条记录缓存基类
    /// </summary>
    public abstract class ANSHCachesRedisCounterBase : ANSHCachesRedisModelBase<long> {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesRedisCounterBase (string cacheKey) : base (cacheKey) {
        }

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public override string CacheKeyPrefix => "counter";

    }
}