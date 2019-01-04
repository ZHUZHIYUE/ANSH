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
    public abstract class ANSHAggregateBase<TAggregateRoot> : IANSHAggregate<TAggregateRoot>
        where TAggregateRoot : class, IANSHAggregateRoot, new () {

            /// <summary>
            /// 当前聚合仓储实现
            /// </summary>
            public abstract IANSHRepository<TAggregateRoot> Repository {
                get;
            }

            /// <summary>
            /// 构造函数
            /// </summary>
            public ANSHAggregateBase () {

            }

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
            /// 删除指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            public virtual void Delete (IANSHSpecificationCommit<TAggregateRoot> specification = null) => Repository.Delete (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TAggregateRoot> GetList (IANSHSpecification<TAggregateRoot> specification = null) => Repository.GetListAsync (specification).Result;

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TAggregateRoot> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20, IANSHSpecification<TAggregateRoot> specification = null) => Repository.GetList (out datacount, out pagecount, out hasnext, page, pagesize, specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual async Task<List<TAggregateRoot>> GetListAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Repository.GetListAsync (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TAggregateRoot GetOne (IANSHSpecification<TAggregateRoot> specification = null) => Repository.GetOne (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual async Task<TAggregateRoot> GetOneAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Repository.GetOneAsync (specification);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual int Count (IANSHSpecification<TAggregateRoot> specification = null) => Repository.Count (specification);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual async Task<int> CountAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Repository.CountAsync (specification);
        }
}