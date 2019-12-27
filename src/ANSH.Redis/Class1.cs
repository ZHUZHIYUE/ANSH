using System;
using StackExchange.Redis;

namespace ANSH.Redis {
    public class Class1 {
        public void a () {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect ("localhost");
            IDatabase db = redis.GetDatabase ();
            db.HashSet ("", new HashEntry[] { new HashEntry ("b", "2"), new HashEntry ("c", "3") });
        }
    }
}