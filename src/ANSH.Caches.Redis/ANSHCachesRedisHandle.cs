using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ANSH.Caches.Redis {

    /// <summary>
    /// 缓存操作
    /// </summary>
    public class ANSHCachesRedisHandle {

        ConnectionMultiplexer _RedisConnection;
        /// <summary>
        /// Redis
        /// </summary>
        ConnectionMultiplexer RedisConnection {
            get {
                if (_RedisConnection == null || !_RedisConnection.IsConnected) {
                    _RedisConnection = ConnectionMultiplexer.Connect (Options);
                }
                return _RedisConnection;
            }
        }

        ConfigurationOptions Options { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHCachesRedisHandle (ConfigurationOptions options) {
            this.Options = options;
        }

        /// <summary>
        /// 插入排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="score">排序</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public bool SortedSetAdd<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, TModel cacheValue, double score, When when = When.Always, bool refresh = false) => SortedSetAddAsync (cachesBase, cacheValue, score, when, refresh).Result;

        /// <summary>
        /// 插入排序数据
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="score">排序</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<bool> SortedSetAddAsync<TModel> (ANSHCachesRedisSortedSetBase<TModel> cachesBase, TModel cacheValue, double score, When when = When.Always, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cachesBase.CacheKey) ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cachesBase.CacheKey)).HasValue)
            ) {
                refresh = true;
            }

            var result = await dataBase.SortedSetAddAsync (cachesBase.CacheKey, StringToModel (cacheValue), score, when);

            if (refresh) {
                await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
            return result;
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
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
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
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            return await dataBase.SortedSetLengthAsync (cachesBase.CacheKey);
        }

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        public long StringIncrement (ANSHCachesRedisCounterBase cachesBase, long step = 1, bool refresh = false) => StringIncrement (cachesBase.CacheKey, step, cachesBase.AbsoluteExpirationRelativeToNow, refresh, cachesBase.DataBaseIndex);

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        public async Task<long> StringIncrementAsync (ANSHCachesRedisCounterBase cachesBase, long step = 1, bool refresh = false) => await StringIncrementAsync (cachesBase.CacheKey, step, cachesBase.AbsoluteExpirationRelativeToNow, refresh, cachesBase.DataBaseIndex);

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cackeKey">缓存Key</param>
        /// <param name="step">步长</param>
        /// <param name="absoluteExpirationRelativeToNow">绝对过期时间</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <param name="dataBaseIndex">DataBase</param>
        public long StringIncrement (string cackeKey, long step = 1, TimeSpan? absoluteExpirationRelativeToNow = null, bool refresh = false, int dataBaseIndex = 0) => StringIncrementAsync (cackeKey, step, absoluteExpirationRelativeToNow, refresh, dataBaseIndex).Result;

        /// <summary>
        /// 计数递增
        /// </summary>
        /// <param name="cackeKey">缓存Key</param>
        /// <param name="step">步长</param>
        /// <param name="absoluteExpirationRelativeToNow">绝对过期时间</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <param name="dataBaseIndex">DataBase</param>
        public async Task<long> StringIncrementAsync (string cackeKey, long step = 1, TimeSpan? absoluteExpirationRelativeToNow = null, bool refresh = false, int dataBaseIndex = 0) {
            var dataBase = RedisConnection.GetDatabase (dataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cackeKey) ||
                (absoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cackeKey)).HasValue)
            ) {
                refresh = true;
            }

            var result = await dataBase.StringIncrementAsync (cackeKey, step);

            if (refresh) {
                await KeyExpireAsync (dataBase, cackeKey, absoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        public long StringDecrement (ANSHCachesRedisCounterBase cachesBase, long step = 1, bool refresh = false) => StringDecrement (cachesBase.CacheKey, step, cachesBase.AbsoluteExpirationRelativeToNow, refresh, cachesBase.DataBaseIndex);

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="step">步长</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        public async Task<long> StringDecrementAsync (ANSHCachesRedisCounterBase cachesBase, long step = 1, bool refresh = false) => await StringDecrementAsync (cachesBase.CacheKey, step, cachesBase.AbsoluteExpirationRelativeToNow, refresh, cachesBase.DataBaseIndex);

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cackeKey">缓存Key</param>
        /// <param name="step">步长</param>
        /// <param name="absoluteExpirationRelativeToNow">绝对过期时间</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <param name="dataBaseIndex">DataBase</param>
        public long StringDecrement (string cackeKey, long step = 1, TimeSpan? absoluteExpirationRelativeToNow = null, bool refresh = false, int dataBaseIndex = 0) => StringDecrementAsync (cackeKey, step, absoluteExpirationRelativeToNow, refresh, dataBaseIndex).Result;

        /// <summary>
        /// 计数递减
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="step">步长</param>
        /// <param name="absoluteExpirationRelativeToNow">绝对过期时间</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <param name="dataBaseIndex">DataBase</param>
        public async Task<long> StringDecrementAsync (string cacheKey, long step = 1, TimeSpan? absoluteExpirationRelativeToNow = null, bool refresh = false, int dataBaseIndex = 0) {

            var dataBase = RedisConnection.GetDatabase (dataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cacheKey) ||
                (absoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cacheKey)).HasValue)
            ) {
                refresh = true;
            }

            var result = await dataBase.StringDecrementAsync (cacheKey, step);

            if (refresh) {
                await KeyExpireAsync (dataBase, cacheKey, absoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        public long ListLength<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => ListLengthAsync (cachesBase).Result;

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        public async Task<long> ListLengthAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase) => await RedisConnection.GetDatabase (cachesBase.DataBaseIndex).ListLengthAsync (cachesBase.CacheKey);

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
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            return (await dataBase.ListRangeAsync (cachesBase.CacheKey, 0, count - 1))?.ToList ().ConvertAll (m => StringToModel<TModel> (m, out _));
        }

        /// <summary>
        /// 顶部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel ListLeftPop<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, bool refresh = false) => ListLeftPopAsync (cachesBase, refresh).Result;

        /// <summary>
        /// 顶部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<TModel> ListLeftPopAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            var result = StringToModel<TModel> (await dataBase.ListLeftPopAsync (cachesBase.CacheKey), out _);
            if (refresh ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !dataBase.KeyTimeToLive (cachesBase.CacheKey).HasValue)
            ) {
                KeyExpire (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// 顶部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public long ListLeftPush<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) => ListLeftPushAsync (cachesBase, cacheValue, when, refresh).Result;

        /// <summary>
        /// 顶部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<long> ListLeftPushAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cachesBase.CacheKey) ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cachesBase.CacheKey)).HasValue)
            ) {
                refresh = true;
            }
            var result = await dataBase.ListLeftPushAsync (cachesBase.CacheKey, StringToModel (cacheValue), when);

            if (refresh) {
                await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// 顶部插入并保留最后插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量，-1为全部保留</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void ListLeftPushAdnTrim<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, long save = -1, bool refresh = false) => ListLeftPushAndTrimAsync (cachesBase, cacheValue, when, save, refresh).Wait ();

        /// <summary>
        /// 顶部插入并保留最后插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task ListLeftPushAndTrimAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, long save = -1, bool refresh = false) => await LockAsync (cachesBase.CacheKey, async () => {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            await ListLeftPushAsync (cachesBase, cacheValue, when, refresh);
            if (save > 0) {
                await dataBase.ListTrimAsync (cachesBase.CacheKey, 0, save);
            }
        });

        /// <summary>
        /// 底部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel ListRightPop<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, bool refresh = false) => ListRightPopAsync (cachesBase, refresh).Result;

        /// <summary>
        /// 底部获取
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<TModel> ListRightPopAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            var result = StringToModel<TModel> (await dataBase.ListRightPopAsync (cachesBase.CacheKey), out _);
            if (refresh ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !dataBase.KeyTimeToLive (cachesBase.CacheKey).HasValue)
            ) {
                KeyExpire (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
            return result;
        }

        /// <summary>
        /// 底部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public long ListRightPush<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) => ListRightPushAsync (cachesBase, cacheValue, when, refresh).Result;

        /// <summary>
        /// 底部插入
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task<long> ListRightPushAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cachesBase.CacheKey) ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cachesBase.CacheKey)).HasValue)
            ) {
                refresh = true;
            }

            var result = await dataBase.ListRightPushAsync (cachesBase.CacheKey, StringToModel (cacheValue), when);

            if (refresh) {
                await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }

            return result;
        }

        /// <summary>
        /// 底部插入并保留最早插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void ListRightPushAdnTrim<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, int save = 0, bool refresh = false) => ListRightPushAndTrimAsync (cachesBase, cacheValue, when, save, refresh).Wait ();

        /// <summary>
        /// 底部插入并保留最早插入数量
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="save">保留数量</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task ListRightPushAndTrimAsync<TModel> (ANSHCachesRedisListBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, int save = 0, bool refresh = false) => await LockAsync (cachesBase.CacheKey, async () => {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            await ListRightPushAsync (cachesBase, cacheValue, when, refresh);
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
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
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
                    var dataBase = RedisConnection.GetDatabase (cacheGroup.Key);
                    await dataBase.KeyDeleteAsync (cacheGroup.Select (m => (RedisKey) m.CacheKey).ToArray ());
                }
            };
        }

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <returns>是否设置成功</returns>
        public bool KeyExpire (ANSHCachesRedisBase cachesBase) => KeyExpireAsync (cachesBase).Result;

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> KeyExpireAsync (ANSHCachesRedisBase cachesBase) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            return await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
        }

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="dataBase">缓存数据库</param>
        /// <param name="key">缓存键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        bool KeyExpire (IDatabase dataBase, RedisKey key, TimeSpan? expiry) => KeyExpireAsync (dataBase, key, expiry).Result;

        /// <summary>
        /// 设置缓存过期时间
        /// </summary>
        /// <param name="dataBase">缓存数据库</param>
        /// <param name="key">缓存键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否设置成功</returns>
        async Task<bool> KeyExpireAsync (IDatabase dataBase, RedisKey key, TimeSpan? expiry) {
            if (expiry.HasValue) {
                return await dataBase.KeyExpireAsync (key, expiry);
            }
            return false;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public void StringSet<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) => StringSetAsync (cachesBase, cacheValue, when, refresh).Wait ();

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cachesBase">缓存内容</param>
        /// <param name="cacheValue">缓存值</param>
        /// <param name="when">何时添加缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public async Task StringSetAsync<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, TModel cacheValue, When when = When.Always, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);

            if (!await dataBase.KeyExistsAsync (cachesBase.CacheKey) ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !(await dataBase.KeyTimeToLiveAsync (cachesBase.CacheKey)).HasValue)
            ) {
                refresh = true;
            }

            await dataBase.StringSetAsync (cachesBase.CacheKey, StringToModel (cacheValue), null, when);

            if (refresh) {
                await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cachesBase">缓存</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public Task<TModel> StringGetAsync<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, out bool breakDown, bool refresh = false) {
            breakDown = true;
            return _StringGetAsync (cachesBase, breakDown, refresh);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cachesBase">缓存</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        async Task<TModel> _StringGetAsync<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, bool breakDown, bool refresh = false) {
            var dataBase = RedisConnection.GetDatabase (cachesBase.DataBaseIndex);
            var cacheValue = await dataBase.StringGetAsync (cachesBase.CacheKey);
            if (refresh ||
                (cachesBase.AbsoluteExpirationRelativeToNow.HasValue && !dataBase.KeyTimeToLive (cachesBase.CacheKey).HasValue)
            ) {
                await KeyExpireAsync (dataBase, cachesBase.CacheKey, cachesBase.AbsoluteExpirationRelativeToNow);
            }
            return StringToModel<TModel> (cacheValue, out breakDown);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cachesBase">缓存</param>
        /// <param name="breakDown">缓存击穿</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel StringGet<TModel> (ANSHCachesRedisModelBase<TModel> cachesBase, out bool breakDown, bool refresh = false) => StringGetAsync (cachesBase, out breakDown, refresh).Result;

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cache">缓存</param>
        /// <param name="refresh">是否刷新缓存时间</param>
        /// <typeparam name="TModel">缓存值模型</typeparam>
        public TModel StringGet<TModel> (ANSHCachesRedisModelBase<TModel> cache, bool refresh = false) => StringGet (cache, out _, refresh);

        // /// <summary>
        // /// 获取缓存
        // /// </summary>
        // /// <param name="cache">缓存</param>
        // /// <param name="refresh">是否刷新缓存时间</param>
        // /// <typeparam name="TModel">缓存值模型</typeparam>
        // public List<TModel> StringGet<TModel> (ANSHCachesRedisModelBase<TModel>[] cache, bool refresh = false) => StringGetAsync (cache, refresh).Result;

        // /// <summary>
        // /// 获取缓存
        // /// </summary>
        // /// <param name="cache">缓存</param>
        // /// <param name="refresh">是否刷新缓存时间</param>
        // /// <typeparam name="TModel">缓存值模型</typeparam>
        // public async Task<List<TModel>> StringGetAsync<TModel> (ANSHCachesRedisModelBase<TModel>[] cache, bool refresh = false) {
        //     List<TModel> result = null;
        //     if (cache?.Length > 0) {
        //         result = new List<TModel> ();
        //         foreach (var cacheGroup in cache.GroupBy (m => m.DataBaseIndex)) {
        //             var dataBase = Redis.GetDatabase (cacheGroup.Key);
        //             var cacheValue = await dataBase.StringGetAsync (cacheGroup.Select (m => (RedisKey) m.CacheKey).ToArray ());
        //             bool refreshItem = refresh;
        //             foreach (var cacheGroupItem in cacheGroup) {
        //                 if (!await KeyExpireAsync (cacheGroupItem)) {
        //                     refreshItem = true;
        //                 }

        //                 if (refreshItem) {
        //                     await KeyExpireAsync (dataBase, cacheGroupItem.CacheKey, cacheGroupItem.AbsoluteExpirationRelativeToNow);
        //                 }
        //             }
        //             foreach (var cacheValueItem in cacheValue) {
        //                 result.Add (StringToModel<TModel> (cacheValueItem.ToString ().ToJson (), out _));
        //             }
        //         }
        //     }
        //     result.RemoveAll (m => m == null);
        //     return result;
        // }

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
            var dataBase = RedisConnection.GetDatabase (0);
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