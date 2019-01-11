using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;
using ANSH.DDD.Domain.Specifications;

namespace ANSH.DDD.Domain.Interface.IAggregates {
    /// <summary>
    /// 聚合
    /// </summary>
    /// <typeparam name="TAggregateRoot">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHAggregate<TAggregateRoot, TPKey> : IANSHQuery<TAggregateRoot, TPKey>
        where TAggregateRoot : class, IEntities.IANSHAggregateRoot<TPKey>
        where TPKey : struct, IEquatable<TPKey>{

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
        }
}