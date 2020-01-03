using System;
using Newtonsoft.Json;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 单条记录缓存基类
    /// </summary>
    /// <typeparam name="TModel">缓存模型</typeparam>
    public abstract class ANSHCachesRedisListBase<TModel> : ANSHCachesRedisBase {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        public ANSHCachesRedisListBase (string cacheKey) : base (cacheKey) {
        }

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public override string CacheKeyPrefix => "list";

    }
}