using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Specifications;

namespace ANSH.DDD.Domain.Server {
    /// <summary>
    /// 聚合根EFCore仓储服务
    /// </summary>
    /// <typeparam name="TIANSHAggregateRootRepository">聚合仓储实现</typeparam>
    /// <typeparam name="TAggregateRoot">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHDomainAggregateServer<TIANSHAggregateRootRepository, TAggregateRoot, TPKey> : IANSHDomainEntityServer<TIANSHAggregateRootRepository, TAggregateRoot, TPKey>
        where TIANSHAggregateRootRepository : IANSHAggregateRootRepository<TAggregateRoot, TPKey>
        where TAggregateRoot : class, IANSHAggregateRoot<TPKey>
        where TPKey : struct, IEquatable<TPKey> {

            /// <summary>
            /// 批量添加实体
            /// </summary>
            /// <param name="TAggregateRoots">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            TAggregateRoot[] Insert (params TAggregateRoot[] TAggregateRoots);

            /// <summary>
            /// 添加实体
            /// </summary>
            /// <param name="action">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            TAggregateRoot Insert (Action<TAggregateRoot> action);

            /// <summary>
            /// 批量添加实体
            /// </summary>
            /// <param name="TAggregateRoot">需要添加的实体</param>
            /// <returns>添加后的实体</returns>
            TAggregateRoot Insert (TAggregateRoot TAggregateRoot);

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="specification">规约</param>
            void Update (Action<TAggregateRoot> action, IANSHSpecificationCommit<TAggregateRoot> specification = null);

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="Id">主键</param>
            void Update (Action<TAggregateRoot> action, TPKey Id);

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            void Delete (IANSHSpecificationCommit<TAggregateRoot> specification = null);

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            void Delete (TPKey Id);

            /// <summary>
            /// 修改指定实体
            /// </summary>
            /// <param name="action">需要修改的项</param>
            /// <param name="criteria">条件</param>
            void Update (Action<TAggregateRoot> action, Expression<Func<TAggregateRoot, bool>> criteria);

            /// <summary>
            /// 删除指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            void Delete (Expression<Func<TAggregateRoot, bool>> criteria);
        }
}