using System;
using System.Threading;
using System.Threading.Tasks;
using ANSH.Caches.Redis;
using StackExchange.Redis;
using Xunit;

namespace Testing.Unit {
    public class ANSHCachesRedisTesting {
        ConnectionMultiplexer _Redis = null;
        ConnectionMultiplexer Redis => _Redis = _Redis?? ConnectionMultiplexer.Connect ("192.168.1.78:7301");

        public class TestListPushAndPopCache : ANSHCachesRedisListBase<int> {

            public TestListPushAndPopCache () : base ("TestListPushAndPopCache") {

            }
        }

        public class TestListPushAndTrimCache : ANSHCachesRedisListBase<int> {

            public TestListPushAndTrimCache () : base ("TestListPushAndTrimCache") {

            }
        }

        public class TestListPushAndRangeCache : ANSHCachesRedisListBase<int> {

            public TestListPushAndRangeCache () : base ("TestListPushAndRangeCache") {

            }
        }

        public class TestSortedSetCache : ANSHCachesRedisSortedSetBase<int> {

            public TestSortedSetCache () : base ("TestSortedSetCache") {

            }
        }

        public class TestStringIncrementCache : ANSHCachesRedisCounterBase {

            public TestStringIncrementCache () : base ("TestStringIncrementCache") {

            }

        }

        public class TestStringIncrementExpirCache : ANSHCachesRedisCounterBase {

            public TestStringIncrementExpirCache () : base ("TestStringIncrementExpirCache") {

            }

            public override TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromSeconds (1);

        }

        [Fact]
        public void TestListPushAndPop () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestListPushAndPopCache (); {
                ANSHCachesRedisHandle.KeyDelete (cacheBase);
                ANSHCachesRedisHandle.ListRightPush (cacheBase, 0);
                ANSHCachesRedisHandle.ListRightPush (cacheBase, 1);
                ANSHCachesRedisHandle.ListRightPush (cacheBase, 2);
                ANSHCachesRedisHandle.ListRightPush (cacheBase, 3);
                Assert.Equal (0, ANSHCachesRedisHandle.ListLeftPop (cacheBase));
                Assert.Equal (1, ANSHCachesRedisHandle.ListLeftPop (cacheBase));
                Assert.Equal (2, ANSHCachesRedisHandle.ListLeftPop (cacheBase));
                Assert.Equal (3, ANSHCachesRedisHandle.ListLeftPop (cacheBase));
                Assert.Equal (0, ANSHCachesRedisHandle.ListLength (cacheBase));
            } {
                ANSHCachesRedisHandle.KeyDelete (cacheBase);
                ANSHCachesRedisHandle.ListLeftPush (cacheBase, 0);
                ANSHCachesRedisHandle.ListLeftPush (cacheBase, 1);
                ANSHCachesRedisHandle.ListLeftPush (cacheBase, 2);
                ANSHCachesRedisHandle.ListLeftPush (cacheBase, 3);
                Assert.Equal (0, ANSHCachesRedisHandle.ListRightPop (cacheBase));
                Assert.Equal (1, ANSHCachesRedisHandle.ListRightPop (cacheBase));
                Assert.Equal (2, ANSHCachesRedisHandle.ListRightPop (cacheBase));
                Assert.Equal (3, ANSHCachesRedisHandle.ListRightPop (cacheBase));
                Assert.Equal (0, ANSHCachesRedisHandle.ListLength (cacheBase));
            }
        }

        [Fact]
        public void TestListPushAndRange () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestListPushAndRangeCache ();
            ANSHCachesRedisHandle.KeyDelete (cacheBase);
            Assert.Equal (0, ANSHCachesRedisHandle.ListLength (cacheBase));

            ANSHCachesRedisHandle.ListRightPush (cacheBase, 0);
            ANSHCachesRedisHandle.ListRightPush (cacheBase, 1);
            ANSHCachesRedisHandle.ListRightPush (cacheBase, 2);
            ANSHCachesRedisHandle.ListRightPush (cacheBase, 3);

            {
                var result = ANSHCachesRedisHandle.ListRange (cacheBase);
                Assert.Equal (ANSHCachesRedisHandle.ListLength (cacheBase), result.Count);
                Assert.Equal (0, result[0]);
                Assert.Equal (1, result[1]);
                Assert.Equal (2, result[2]);
                Assert.Equal (3, result[3]);
            } {
                var result = ANSHCachesRedisHandle.ListRange (cacheBase, 2);
                Assert.Equal (2, result.Count);
                Assert.Equal (0, result[0]);
                Assert.Equal (1, result[1]);
            }
        }

        [Fact]
        public void TestListPushAndTrim () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestListPushAndTrimCache ();
            ANSHCachesRedisHandle.KeyDelete (cacheBase);
            Assert.Equal (0, ANSHCachesRedisHandle.ListLength (cacheBase));

            ANSHCachesRedisHandle.ListRightPushAdnTrim (cacheBase, 0, save : 1);
            ANSHCachesRedisHandle.ListRightPushAdnTrim (cacheBase, 1, save : 1);
            ANSHCachesRedisHandle.ListRightPushAdnTrim (cacheBase, 2, save : 1);
            ANSHCachesRedisHandle.ListRightPushAdnTrim (cacheBase, 3, save : 1);

            var result = ANSHCachesRedisHandle.ListRange (cacheBase);

            Assert.Equal (1, ANSHCachesRedisHandle.ListLength (cacheBase));
            Assert.Equal (1, result.Count);
            Assert.Equal (0, result[0]);
        }

        [Fact]
        public void TestSorted () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestSortedSetCache ();
            ANSHCachesRedisHandle.KeyDelete (cacheBase);
            Assert.Equal (0, ANSHCachesRedisHandle.SortedSetLength (cacheBase));

            ANSHCachesRedisHandle.SortedSetAdd (cacheBase, 0, 1);
            ANSHCachesRedisHandle.SortedSetAdd (cacheBase, 1, 0);

            {
                var result = ANSHCachesRedisHandle.SortedSetRangeByRank (cacheBase);
                Assert.Equal (ANSHCachesRedisHandle.SortedSetLength (cacheBase), result.Count);
                Assert.Equal (2, result.Count);
                Assert.Equal (1, result[0]);
                Assert.Equal (0, result[1]);
            }

            {
                var result = ANSHCachesRedisHandle.SortedSetRangeByRank (cacheBase, Order.Descending);
                Assert.Equal (ANSHCachesRedisHandle.SortedSetLength (cacheBase), result.Count);
                Assert.Equal (2, result.Count);
                Assert.Equal (0, result[0]);
                Assert.Equal (1, result[1]);
            }
        }

        [Fact]
        public void TestStringIncrement () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestStringIncrementCache ();
            ANSHCachesRedisHandle.KeyDelete (cacheBase);

            {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (1, result);
            }

            {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (2, result);
            }

            {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (3, result);
            }
        }

        [Fact]
        public void TestStringIncrementExpir () {
            var ANSHCachesRedisHandle = new ANSHCachesRedisHandle (Redis);
            var cacheBase = new TestStringIncrementExpirCache ();
            ANSHCachesRedisHandle.KeyDelete (cacheBase);

            {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (1, result);
                Thread.Sleep (1000);
            } {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (1, result);
                Thread.Sleep (500);
            } {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase, refresh : true);
                Assert.Equal (2, result);
                Thread.Sleep (500);
            } {
                var result = ANSHCachesRedisHandle.StringIncrement (cacheBase);
                Assert.Equal (3, result);
                Thread.Sleep (500);
            }
        }
    }
}