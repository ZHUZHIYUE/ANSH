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
        /// 缓存键
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public abstract string CacheKey { get; }
    }
}