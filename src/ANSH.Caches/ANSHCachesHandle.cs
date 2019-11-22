using Microsoft.Extensions.Caching.Distributed;

namespace ANSH.Caches {

    /// <summary>
    /// 缓存操作
    /// </summary>
    public class ANSHCachesHandle {

        /// <summary>
        /// 缓存操作接口
        /// </summary>
        /// <value></value>
        IDistributedCache DistributedCache { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="distributedCache">缓存操作接口</param>
        public ANSHCachesHandle (IDistributedCache distributedCache) {
            DistributedCache = distributedCache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TANSHCachesBase">缓存基类</typeparam>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Set<TANSHCachesBase, TModel> (TANSHCachesBase cachesBase) where TANSHCachesBase : ANSHCachesModelBase<TModel> {
            if (cachesBase.CachesValue != null) {
                DistributedCache.SetString (cachesBase.CacheKey, cachesBase.CachesValue.ToJson (), cachesBase.CacheOptions);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="existsCachesBase">返回已存在相同CacheKey的缓存内容</param>
        /// <param name="replace">若有相同CacheKey的缓存内容是否替换</param>
        /// <typeparam name="TANSHCachesBase">缓存基类</typeparam>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Set<TANSHCachesBase, TModel> (TANSHCachesBase cachesBase, out TANSHCachesBase existsCachesBase, bool replace = true) where TANSHCachesBase : ANSHCachesModelBase<TModel> {

            existsCachesBase = Get<TANSHCachesBase, TModel> (cachesBase);

            if (replace) {
                Set<TANSHCachesBase, TModel> (cachesBase);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TANSHCachesBase">缓存基类</typeparam>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TANSHCachesBase Get<TANSHCachesBase, TModel> (TANSHCachesBase cache) where TANSHCachesBase : ANSHCachesModelBase<TModel> {
            string cacheValue = DistributedCache.GetString (cache.CacheKey);
            if (!string.IsNullOrWhiteSpace (cacheValue)) {
                cache.CachesValue = cacheValue.ToJsonObj<TModel> ();
            } else {
                cache.CachesValue = default (TModel);
            }
            return cache;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TANSHCachesBase">缓存基类</typeparam>
        public void Remove<TANSHCachesBase> (TANSHCachesBase cache) where TANSHCachesBase : ANSHCachesBase => DistributedCache.Remove (cache.CacheKey);
    }
}