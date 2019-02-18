using System;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;

namespace ANSH.DDD.Domain.Repository.EFCore {
    /// <summary>
    /// 聚合根EFCore仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    /// <typeparam name="TRepositoryUnitOfWork">EFCore仓储操作工作基类接口</typeparam>
    public interface IANSHEFCoreAggregateRootRepository<TAggregateRoot, TPKey, TRepositoryUnitOfWork> : IANSHEFCoreRepository<TAggregateRoot, TPKey, TRepositoryUnitOfWork>, IANSHAggregateRootRepository<TAggregateRoot, TPKey>
        where TAggregateRoot : ANSHEFCoreAggregateRootBase<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey>
        where TRepositoryUnitOfWork : IANSHEFCoreUnitOfWork, IANSHRepositoryUnitOfWork { }
}