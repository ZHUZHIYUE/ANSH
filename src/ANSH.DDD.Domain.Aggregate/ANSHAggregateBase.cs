using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ANSH.DataBase.IUnitOfWorks;
using ANSH.DDD.Domain.Interface.IAggregates;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;
using ANSH.DDD.Domain.Specifications;

namespace ANSH.DDD.Domain.Aggregate {
    /// <summary>
    /// 聚合的抽象实现类，定义聚合的公共属性和行为
    /// </summary>
    /// <typeparam name="TAggregateRoot">泛型聚合根，因为在DDD里面仓储只能对聚合根做操作</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public class ANSHAggregateBase<TAggregateRoot, TPKey> : ANSHQueryBase<TAggregateRoot, TPKey>, IANSHAggregate<TAggregateRoot, TPKey>
        where TAggregateRoot : class, IANSHAggregateRoot<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey> {

        /// <summary>
        /// 当前聚合仓储实现
        /// </summary>
        new protected IANSHAggregateRootRepository<TAggregateRoot, TPKey> Repository {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">仓储</param>
        public ANSHAggregateBase (IANSHAggregateRootRepository<TAggregateRoot, TPKey> repository) : base (repository) { }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="TAggregateRoots">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TAggregateRoot[] Insert (params TAggregateRoot[] TAggregateRoots) => Repository.Insert (TAggregateRoots);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="action">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TAggregateRoot Insert (Action<TAggregateRoot> action) => Repository.Insert (action);

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="action">需要修改的项</param>
        /// <param name="specification">规约</param>
        public virtual void Update (Action<TAggregateRoot> action, IANSHSpecificationCommit<TAggregateRoot> specification = null) => Repository.Update (action, specification);

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="action">需要修改的项</param>
        /// <param name="Id">主键</param>
        public void Update (Action<TAggregateRoot> action, TPKey Id) => Repository.Update (action, Id);

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        public virtual void Delete (IANSHSpecificationCommit<TAggregateRoot> specification = null) => Repository.Delete (specification);

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        public void Delete (TPKey Id) => Repository.Delete (Id);
    }
}