using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;
using ANSH.DDD.Domain.Specifications;
using ANSH.DDD.Domain.Specifications.EFCore;

namespace ANSH.DDD.Domain.Repository.EFCore {
    
    /// <summary>
    /// 聚合根EFCore仓储实现基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    /// <typeparam name="TAggregateRootContxt">EFCore实现</typeparam>
    public class ANSHEFCoreAggregateRootRepositoryBase<TAggregateRoot, TPKey, TAggregateRootContxt> : ANSHEFCoreRepositoryBase<TAggregateRoot, TPKey, TAggregateRootContxt>, IANSHEFCoreAggregateRootRepository<TAggregateRoot, TPKey>
        where TAggregateRoot : ANSHEFCoreAggregateRootBase<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey>
        where TAggregateRootContxt : ANSHDbContextOptionsBase<TAggregateRoot>, new () {

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="work">EFCore仓储操作工作类</param>
            public ANSHEFCoreAggregateRootRepositoryBase (IANSHEFCoreUnitOfWork work) : base (work) { }

            /// <summary>
            /// 批量添加实体
            /// </summary>
            /// <param name="TAggregateRoots">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            public virtual TAggregateRoot[] Insert (params TAggregateRoot[] TAggregateRoots) => Work.Register<TAggregateRootContxt> ().Insert (TAggregateRoots);

            /// <summary>
            /// 批量添加实体
            /// </summary>
            /// <param name="model">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            public TAggregateRoot Insert (TAggregateRoot model) => Work.Register<TAggregateRootContxt> ().Insert (model);

            /// <summary>
            /// 添加实体
            /// </summary>
            /// <param name="action">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            public virtual TAggregateRoot Insert (Action<TAggregateRoot> action) => Work.Register<TAggregateRootContxt> ().Insert (action);

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="criteria">条件</param>
            public virtual void Update (Action<TAggregateRoot> action, Expression<Func<TAggregateRoot, bool>> criteria) {
                var specification = new ANSHSpecificationCommitBase<TAggregateRoot> ();
                specification.SetCriteria (criteria);
                Update (action, specification);
            }

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="specification">规约</param>
            public virtual void Update (Action<TAggregateRoot> action, IANSHSpecificationCommit<TAggregateRoot> specification = null) => Work.Register<TAggregateRootContxt> ().Update (action, specification);

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="Id">主键</param>
            public virtual void Update (Action<TAggregateRoot> action, TPKey Id) => Update (action, m => m.Id.Equals (Id));

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            public virtual void Delete (Expression<Func<TAggregateRoot, bool>> criteria) {
                var specification = new ANSHSpecificationCommitBase<TAggregateRoot> ();
                specification.SetCriteria (criteria);
                Delete (specification);
            }

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            public virtual void Delete (IANSHSpecificationCommit<TAggregateRoot> specification = null) => Work.Register<TAggregateRootContxt> ().Delete (specification);

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            public virtual void Delete (TPKey Id) => Delete (m => m.Id.Equals (Id));
        }
}