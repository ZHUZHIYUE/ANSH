using System;

namespace ANSH.DDD.Domain.Interface.IEntities {
    /// <summary>
    /// 聚合根，继承于该接口的对象是外部唯一操作的对象
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHAggregateRoot<TPKey> : IANSHEntity<TPKey> where TPKey :struct, IEquatable<TPKey> {

    }
}