using System;
using ANSH.DataBase.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;

namespace ANSH.DDD.Domain.Entities.EFCore {
    /// <summary>
    /// EFCore实体类的接口，定义实体类的公共属性和行为
    /// </summary>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHEFCoreEntity<TPKey> : IANSHEntity<TPKey>, IDBDomainEntity<TPKey> where TPKey : struct, IEquatable<TPKey> {

    }
}