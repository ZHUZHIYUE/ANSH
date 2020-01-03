using System;
using Newtonsoft.Json;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 单条记录缓存基类
    /// </summary>
    /// <typeparam name="TModel">缓存模型</typeparam>
    public abstract class ANSHCachesRedisModelBase<TModel> : ANSHCachesRedisBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesRedisModelBase (string cacheKey) : base (cacheKey) { }

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public override string CacheKeyPrefix => "string";

        /// <summary>
        /// 过期时间
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public abstract TimeSpan? AbsoluteExpirationRelativeToNow { get; }

    }
}