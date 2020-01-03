using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ANSH.Caches.DistributedCache {

    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class ANSHCachesDistributedCacheBase : ANSHCachesBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHCachesDistributedCacheBase () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesDistributedCacheBase (string cacheKey) : base () {
            this.CacheKey = cacheKey;
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public override string CacheKey { get; }

        /// <summary>
        /// 缓存配置
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public abstract DistributedCacheEntryOptions CacheOptions { get; }
    }
}