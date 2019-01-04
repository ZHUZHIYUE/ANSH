using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;
using ANSH.DDD.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DDD.Domain.Repository.EFCore {
    /// <summary>
    /// EFCore仓储实现基类
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根</typeparam>
    /// <typeparam name="TRepositoryUnitOfWork">EFCore仓储操作工作基类接口</typeparam>
    /// <typeparam name="TAggregateRootContxt">EFCore实现</typeparam>
    public abstract class ANSHEFCoreRepositoryBase<TAggregateRoot, TRepositoryUnitOfWork, TAggregateRootContxt> : IANSHEFCoreRepository<TAggregateRoot, TRepositoryUnitOfWork>
        where TAggregateRoot : class, IDBEntity, IANSHAggregateRoot, new ()
    where TRepositoryUnitOfWork : IANSHEFCoreUnitOfWork, IANSHRepositoryUnitOfWork
    where TAggregateRootContxt : DBContextOptions<TAggregateRoot>, new () {

        /// <summary>
        /// EFCore仓储操作工作类
        /// </summary>
        public TRepositoryUnitOfWork Work {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="work">EFCore仓储操作工作类</param>
        public ANSHEFCoreRepositoryBase (TRepositoryUnitOfWork work) {
            Work = work;
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="TAggregateRoots">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public TAggregateRoot[] Insert (params TAggregateRoot[] TAggregateRoots) => Work.Register<TAggregateRootContxt> ().Insert (TAggregateRoots);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="action">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public TAggregateRoot Insert (Action<TAggregateRoot> action) => Work.Register<TAggregateRootContxt> ().Insert (action);

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="action">需要修改的项</param>
        /// <param name="specification">规约</param>
        public void Update (Action<TAggregateRoot> action, IANSHSpecificationCommit<TAggregateRoot> specification = null) {
            Work.Register<TAggregateRootContxt> ().Update (action, specification);
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        public void Delete (IANSHSpecificationCommit<TAggregateRoot> specification = null) {
            Work.Register<TAggregateRootContxt> ().Delete (specification);
        }

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public List<TAggregateRoot> GetList (IANSHSpecification<TAggregateRoot> specification = null) => GetListAsync (specification).Result;

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
        public List<TAggregateRoot> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20, IANSHSpecification<TAggregateRoot> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ().Result;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public async Task<List<TAggregateRoot>> GetListAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).ToListAsync ();

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public TAggregateRoot GetOne (IANSHSpecification<TAggregateRoot> specification = null) => GetOneAsync (specification).Result;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public async Task<TAggregateRoot> GetOneAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).FirstOrDefaultAsync ();

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        public int Count (IANSHSpecification<TAggregateRoot> specification = null) => CountAsync (specification).Result;

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        public async Task<int> CountAsync (IANSHSpecification<TAggregateRoot> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).CountAsync ();
    }
}