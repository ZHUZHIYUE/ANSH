using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;

namespace ANSH.DDD.Domain.Repository.EFCore {
    /// <summary>
    /// EFCore仓储接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    /// <typeparam name="TRepositoryUnitOfWork">EFCore仓储操作工作基类接口</typeparam>
    public interface IANSHEFCoreRepository<TAggregateRoot, TRepositoryUnitOfWork> : IANSHRepository<TAggregateRoot>
        where TAggregateRoot : class, IDBEntity, IANSHAggregateRoot, new ()
    where TRepositoryUnitOfWork : IANSHEFCoreUnitOfWork, IANSHRepositoryUnitOfWork {

        /// <summary>
        /// EFCore仓储操作工作类
        /// </summary>
        TRepositoryUnitOfWork Work {
            get;
        }
    }
}