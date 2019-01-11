using System;

namespace ANSH.DDD.Domain.Interface.IEntities {
    /// <summary>
    /// 领域实体接口，继承自该接口的为领域实体
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHEntity<TPKey> where TPKey : struct, IEquatable<TPKey> {

        /// <summary>
        /// 主键
        /// </summary>
        TPKey? Id {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? create_times {
            get;
            set;
        }
    }
}