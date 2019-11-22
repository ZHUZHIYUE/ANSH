using System;

namespace ANSH.DDD.Domain.Entities.EFCore {
    /// <summary>
    ///  EFCore实体类的抽象实现类，定义实体类的公共属性和行为
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public class ANSHEFCoreEntityBase<TPKey> : IANSHEFCoreEntity<TPKey> where TPKey : struct, IEquatable<TPKey> {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPKey? Id {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateTimes {
            get;
            set;
        }

        /// <summary>
        /// 最近一次修改时间
        /// </summary>
        public virtual DateTime? UpdateTimes {
            get;
            set;
        }

        /// <summary>
        /// 行版本控制
        /// </summary>
        public virtual byte[] Timestamp {
            get;
            set;
        }
    }
}