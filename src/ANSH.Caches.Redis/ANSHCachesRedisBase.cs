using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class ANSHCachesRedisBase : ANSHCachesBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesRedisBase (string cacheKey) : base () {
            this.CacheKey = $"{CacheKeyPrefix}-{cacheKey}";
        }

        /// <summary>
        /// 存储数据库位置
        /// </summary>
        [JsonIgnore]
        public virtual int DataBaseIndex { get; set; } = 0;

        /// <summary>
        /// 缓存键
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public override string CacheKey { get; }

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public abstract string CacheKeyPrefix { get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public virtual TimeSpan? AbsoluteExpirationRelativeToNow { get; }
    }
}