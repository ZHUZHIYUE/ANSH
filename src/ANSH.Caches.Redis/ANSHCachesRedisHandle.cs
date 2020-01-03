﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 缓存操作
    /// </summary>
    public class ANSHCachesRedisHandle {
        /// <summary>
        /// Redis
        /// </summary>
        ConnectionMultiplexer Redis { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHCachesRedisHandle (ConnectionMultiplexer redis) {
            this.Redis = redis;
        }

        /// <summary>
        /// 插入排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="score">排序</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public bool SortedSetAdd<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, TModel cacheValue, double score, When when = When.Always) => SortedSetAddAsync (cachesBase, cacheValue, score, when).Result;

        /// <summary>
        /// 插入排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="score">排序</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<bool> SortedSetAddAsync<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, TModel cacheValue, double score, When when = When.Always) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.SortedSetAddAsync (cachesBase.CacheKey, StringToModel (cacheValue), score, when);
        }

        /// <summary>
        /// 获取排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="order">排序</param>
        /// <param name="count">取几条，0为获取所有</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public List<TModel> SortedSetRangeByRank<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, Order order = Order.Ascending, long count = 0) => SortedSetRangeByRankAsync (cachesBase, order, count).Result;

        /// <summary>
        /// 获取排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="order">排序</param>
        /// <param name="count">取几条，0为获取所有</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<List<TModel>> SortedSetRangeByRankAsync<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, Order order = Order.Ascending, long count = 0) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return (await dataBase.SortedSetRangeByRankAsync (cachesBase.CacheKey, 0, count - 1, order))?.ToList ().ConvertAll (m => StringToModel<TModel> (m, out _));
        }

        /// <summary>
        /// 获取排序数据数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public long SortedSetLength<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase) => SortedSetLengthAsync (cachesBase).Result;

        /// <summary>
        /// 获取排序数据数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<long> SortedSetLengthAsync<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.SortedSetLengthAsync (cachesBase.CacheKey);
        }

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        public long StringIncrement (ANSHCachesRedisBase cachesBase, long step = 1) => StringIncrementAsync (cachesBase, step).Result;

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        public async Task<long> StringIncrementAsync (ANSHCachesRedisBase cachesBase, long step = 1) => await Redis.GetDatabase (cachesBase.DataBaseIndex).StringIncrementAsync (cachesBase.CacheKey, step);

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        public long StringDecrement (ANSHCachesRedisBase cachesBase, long step = 1) => StringDecrementAsync (cachesBase, step).Result;

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        public async Task<long> StringDecrementAsync (ANSHCachesRedisBase cachesBase, long step = 1) => await Redis.GetDatabase (cachesBase.DataBaseIndex).StringDecrementAsync (cachesBase.CacheKey, step);

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        public long ListLength<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => ListLengthAsync (cachesBase).Result;

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        public async Task<long> ListLengthAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => await Redis.GetDatabase (cachesBase.DataBaseIndex).ListLengthAsync (cachesBase.CacheKey);

        /// <summary>
        /// 获取指定数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="count">取出数量，0为全部取出</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public List<TModel> ListRange<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, long count = 0) => ListRangeAsync (cachesBase, count).Result;

        /// <summary>
        /// 获取指定数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="count">取出数量，0为全部取出</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<List<TModel>> ListRangeAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, long count = 0) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return (await dataBase.ListRangeAsync (cachesBase.CacheKey, 0, count - 1))?.ToList ().ConvertAll (m => StringToModel<TModel> (m, out _));
        }

        /// <summary>
        /// 顶部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel ListLeftPop<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => ListLeftPopAsync (cachesBase).Result;

        /// <summary>
        /// 顶部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<TModel> ListLeftPopAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return StringToModel<TModel> (await dataBase.ListLeftPopAsync (cachesBase.CacheKey), out _);
        }

        /// <summary>
        /// 顶部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public long ListLeftPush<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) => ListLeftPushAsync (cachesBase, cacheValue, when).Result;

        /// <summary>
        /// 顶部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<long> ListLeftPushAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.ListLeftPushAsync (cachesBase.CacheKey, StringToModel (cacheValue), when);
        }

        /// <summary>
        /// 顶部插入并保留最后插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量，-1为全部保留</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void ListLeftPushAdnTrim<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, long save = -1) => ListLeftPushAndTrimAsync (cachesBase, cacheValue, when, save).Wait ();

        /// <summary>
        /// 顶部插入并保留最后插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task ListLeftPushAndTrimAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, long save = -1) => await LockAsync (cachesBase.CacheKey, async () => {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            await dataBase.ListLeftPushAsync (cachesBase.CacheKey, StringToModel (cacheValue), when);
            if (save > 0) {
                await dataBase.ListTrimAsync (cachesBase.CacheKey, 0, save);
            }
        });

        /// <summary>
        /// 底部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel ListRightPop<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => ListRightPopAsync (cachesBase).Result;

        /// <summary>
        /// 底部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<TModel> ListRightPopAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return StringToModel<TModel> (await dataBase.ListRightPopAsync (cachesBase.CacheKey), out _);
        }

        /// <summary>
        /// 底部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public long ListRightPush<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) => ListRightPushAsync (cachesBase, cacheValue, when).Result;

        /// <summary>
        /// 底部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<long> ListRightPushAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.ListRightPushAsync (cachesBase.CacheKey, StringToModel (cacheValue), when);
        }

        /// <summary>
        /// 底部插入并保留最早插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void ListRightPushAdnTrim<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, int save = 0) => ListRightPushAndTrimAsync (cachesBase, cacheValue, when, save).Wait ();

        /// <summary>
        /// 底部插入并保留最早插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task ListRightPushAndTrimAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, int save = 0) => await LockAsync (cachesBase.CacheKey, async () => {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            await ListRightPushAsync (cachesBase, cacheValue, when);
            if (save > 0) {
                await dataBase.ListTrimAsync (cachesBase.CacheKey, 0, save - 1);
            }
        });

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="when">何时添加缓存</param>
        public bool KeyDelete (ANSHCachesRedisBase cachesBase, When when = When.Always) => KeyDeleteAsync (cachesBase, when).Result;

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="when">何时添加缓存</param>
        public async Task<bool> KeyDeleteAsync (ANSHCachesRedisBase cachesBase, When when = When.Always) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.KeyDeleteAsync (cachesBase.CacheKey);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="when">何时添加缓存</param>
        public void KeyDelete (ANSHCachesRedisBase[] cachesBase, When when = When.Always) => KeyDeleteAsync (cachesBase, when).Wait ();

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="when">何时添加缓存</param>
        public async Task KeyDeleteAsync (ANSHCachesRedisBase[] cachesBase, When when = When.Always) {
            if (cachesBase?.Length > 0) {
                foreach (var cacheGroup in cachesBase.GroupBy (m => m.DataBaseIndex)) {
                    var dataBase = Redis.GetDatabase (cacheGroup.Key);
                    await dataBase.KeyDeleteAsync (cacheGroup.Select (m => (RedisKey) m.CacheKey).ToArray ());
                }
            };
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void StringSet<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) => StringSetAsync (cachesBase, cacheValue, when).Wait ();

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task StringSetAsync<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, TModel cacheValue, When when = When.Always) {
            var dataBase = Redis.GetDatabase (cachesBase.DataBaseIndex);
            await dataBase.StringSetAsync (cachesBase.CacheKey, StringToModel (cacheValue), cachesBase.AbsoluteExpirationRelativeToNow, when);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<TModel> StringGetAsync<TModel> (ANSHCachesRedisModelBase<TModel> cache) {
            await Task.CompletedTask;
            return StringGet (cache, out _);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel StringGet<TModel> (ANSHCachesRedisModelBase<TModel> cache, out bool breakDown) {
            var dataBase = Redis.GetDatabase (cache.DataBaseIndex);
            var cacheValue = dataBase.StringGetAsync (cache.CacheKey).Result;
            return StringToModel<TModel> (cacheValue, out breakDown);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel StringGet<TModel> (ANSHCachesRedisModelBase<TModel> cache) => StringGet (cache, out _);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public List<TModel> StringGet<TModel> (ANSHCachesRedisModelBase<TModel>[] cache) => StringGetAsync (cache).Result;

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<List<TModel>> StringGetAsync<TModel> (ANSHCachesRedisModelBase<TModel>[] cache) {
            List<TModel> result = null;
            if (cache?.Length > 0) {
                result = new List<TModel> ();
                foreach (var cacheGroup in cache.GroupBy (m => m.DataBaseIndex)) {
                    var dataBase = Redis.GetDatabase (cacheGroup.Key);
                    foreach (var cacheGroupItem in cacheGroup) { }
                    var cacheValue = await dataBase.StringGetAsync (cacheGroup.Select (m => (RedisKey) m.CacheKey).ToArray ());
                    foreach (var cacheValueItem in cacheValue) {
                        result.Add (StringToModel<TModel> (cacheValueItem.ToString ().ToJson (), out _));
                    }
                }
            }
            result.RemoveAll (m => m == null);
            return result;
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁的名称</param>
        /// <param name="action">操作</param>
        public void Lock (string lockKey, Action action) => LockAsync (lockKey, action).Wait ();

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁的名称</param>
        /// <param name="action">操作</param>
        /// <param name="expire">过期时间，单位表</param>
        public async Task LockAsync (string lockKey, Action action, int expire = 10) {
            var dataBase = Redis.GetDatabase (0);
            var token = Guid.NewGuid ().ToString ();
            bool hasLock = false;
            DateTime expired = DateTime.Now.AddSeconds (expire);
            while (!hasLock) {
                if (expired < DateTime.Now) {
                    throw new TimeoutException ("等待超时");
                }
                hasLock = await dataBase.LockTakeAsync ($"{lockKey}_lock", token, TimeSpan.FromSeconds (expire));
                if (hasLock) {
                    try {
                        action ();
                    } catch (Exception ex) {
                        throw ex;
                    } finally {
                        await dataBase.LockReleaseAsync ($"{lockKey}_lock", token);
                    }
                }
            }
        }

        /// <summary>
        /// 转换模型
        /// </summary>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        TModel StringToModel<TModel> (string cacheValue, out bool breakDown) {
            if (!string.IsNullOrWhiteSpace (cacheValue)) {
                breakDown = false;
                return cacheValue == "unknown" ? default (TModel) : cacheValue.ToJsonObj<TModel> ();
            } else {
                breakDown = true;
                return default (TModel);
            }
        }

        /// <summary>
        /// 转换模型
        /// </summary>
        /// <param name="cacheValue">缓存值</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        string StringToModel<TModel> (TModel cacheValue) => cacheValue?.ToJson () ?? "unknown";
    }
}