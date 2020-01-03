using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ANSH.Caches.DistributedCache {

    /// <summary>
    /// 缓存操作
    /// </summary>
    public class ANSHCachesDistributedCacheHandle {

        /// <summary>
        /// 缓存操作接口
        /// </summary>
        /// <value></value>
        IDistributedCache DistributedCache { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="distributedCache">缓存操作接口</param>
        public ANSHCachesDistributedCacheHandle (IDistributedCache distributedCache) {
            DistributedCache = distributedCache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Set<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cachesBase, bool refresh = false) => SetAsync (cachesBase, refresh).Wait ();

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task SetAsync<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cachesBase, bool refresh = false) {
            if (refresh || Get (cachesBase) == null) {
                await DistributedCache.SetStringAsync (cachesBase.CacheKey, cachesBase.CachesValue?.ToJson () ?? "unknown", cachesBase.CacheOptions);
            } else {
                await DistributedCache.SetStringAsync (cachesBase.CacheKey, cachesBase.CachesValue?.ToJson () ?? "unknown");
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="existsCachesBase">返回已存在相同CacheKey的缓存内容</param>
        /// <param name="replace">若有相同CacheKey的缓存内容是否替换</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Set<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cachesBase, out TModel existsCachesBase, bool replace = true, bool refresh = false) {

            existsCachesBase = Get (cachesBase);

            if (replace) {
                Set (cachesBase);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel Get<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cache, out bool breakDown) {
            string cacheValue = DistributedCache.GetStringAsync (cache.CacheKey).Result;
            if (!string.IsNullOrWhiteSpace (cacheValue)) {
                breakDown = false;
                return cacheValue == "unknown" ? default (TModel) : cacheValue.ToJsonObj<TModel> ();
            } else {
                breakDown = true;
                return default (TModel);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel Get<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cache) => Get (cache, out _);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Remove<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cache) => RemoveAsync (cache).Wait ();

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task RemoveAsync<TModel> (ANSHCachesDistributedCacheModelBase<TModel> cache) => await DistributedCache.RemoveAsync (cache.CacheKey);
    }
}