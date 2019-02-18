using System;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;

namespace ANSH.DDD.Domain.Repository.EFCore {
    /// <summary>
    /// 实体类EFCore仓储接口
    /// </summary>
    /// <typeparam name="Entity">泛型实体类，因为在DDD里面仓储只能对实体类做操作</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    /// <typeparam name="TRepositoryUnitOfWork">EFCore仓储操作工作基类接口</typeparam>
    public interface IANSHEFCoreRepository<Entity, TPKey, TRepositoryUnitOfWork> : IANSHRepository<Entity, TPKey>
        where Entity : ANSHEFCoreEntityBase<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey>
        where TRepositoryUnitOfWork : IANSHEFCoreUnitOfWork, IANSHRepositoryUnitOfWork {

            /// <summary>
            /// EFCore仓储操作工作类
            /// </summary>
            TRepositoryUnitOfWork Work {
                get;
            }
        }
}