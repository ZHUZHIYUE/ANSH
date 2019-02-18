using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace ANSH.AspNetCore.Middlewares {
    /// <summary>
    /// 使用Redis保存Keys
    /// </summary>
    public class RedisXmlRepository : IXmlRepository {

        /// <summary>
        /// Redis Key
        /// </summary>
        string CacheKey {
            get;
            set;
        }

        /// <summary>
        /// Reids
        /// </summary>
        IDistributedCache Distributed {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="distributed">Redis分布式缓存</param>
        /// <param name="cachekey">缓存KEY</param>
        public RedisXmlRepository (IDistributedCache distributed, string cachekey) {
            Distributed = distributed;
            CacheKey = cachekey;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns>XELement</returns>
        public IReadOnlyCollection<XElement> GetAllElements () {
            return new List<XElement> () { XElement.Parse (Distributed.GetString (CacheKey) ?? "<?xml version=\"1.0\" encoding=\"utf-8\"?><key></key>") };
        }

        /// <summary>
        /// 保存值
        /// </summary>
        /// <param name="element">XELement</param>
        /// <param name="friendlyName">key</param>
        public void StoreElement (XElement element, string friendlyName) {
            if (element == null) {
                throw new ArgumentNullException (nameof (element));
            }
            if (string.IsNullOrWhiteSpace (friendlyName)) {
                friendlyName = Guid.NewGuid ().ToString ();
            }
            Distributed.SetString (CacheKey, element.ToString ());
        }
    }
}