namespace ANSH.Caches {

    /// <summary>
    /// 单条记录缓存基类
    /// </summary>
    /// <typeparam name="TModel">缓存模型</typeparam>
    public abstract class ANSHCachesModelBase<TModel> : ANSHCachesBase {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHCachesModelBase () : base () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheValue">缓存值</param>
        public ANSHCachesModelBase (string cacheKey, TModel cacheValue) : base (cacheKey) {
            CachesValue = cacheValue;
        }

        /// <summary>
        /// 缓存值
        /// </summary>
        /// <value></value>
        public TModel CachesValue { get; set; }

    }
}