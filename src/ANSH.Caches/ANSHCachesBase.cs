using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ANSH.Caches {

    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class ANSHCachesBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHCachesBase () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesBase (string cacheKey) {
            CacheKey = cacheKey;
        }

        /// <summary>
        /// 缓存键
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public virtual string CacheKey { get; }

        /// <summary>
        /// 缓存配置
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public abstract DistributedCacheEntryOptions CacheOptions { get; }
    }
}