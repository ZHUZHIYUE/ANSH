﻿using Microsoft.Extensions.Caching.Distributed;

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
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Set<TModel> (ANSHCachesModelBase<TModel> cachesBase, bool refresh = false) {
            if (refresh || Get (cachesBase) == null) {
                DistributedCache.SetString (cachesBase.CacheKey, cachesBase.CachesValue?.ToJson () ?? "unknown", cachesBase.CacheOptions);
            } else {
                DistributedCache.SetString (cachesBase.CacheKey, cachesBase.CachesValue?.ToJson () ?? "unknown");
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
        public void Set<TModel> (ANSHCachesModelBase<TModel> cachesBase, out TModel existsCachesBase, bool replace = true, bool refresh = false) {

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
        public TModel Get<TModel> (ANSHCachesModelBase<TModel> cache, out bool breakDown) {
            string cacheValue = DistributedCache.GetString (cache.CacheKey);
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
        public TModel Get<TModel> (ANSHCachesModelBase<TModel> cache) => Get (cache, out _);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void Remove<TModel> (ANSHCachesModelBase<TModel> cache) => DistributedCache.Remove (cache.CacheKey);
    }
}