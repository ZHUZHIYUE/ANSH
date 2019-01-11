using System;
using ANSH.DataBase.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;

namespace ANSH.DDD.Domain.Entities.EFCore {
    /// <summary>
    /// EFCore聚合根的接口，定义聚合根的公共属性和行为
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHEFCoreAggregateRoot<TPKey> : IANSHEFCoreEntity<TPKey>, IANSHAggregateRoot<TPKey> where TPKey : struct, IEquatable<TPKey> {

    }
}