using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks;
using ANSH.DataBase.IUnitOfWorks.EFCore;
using ANSH.DDD.Domain.Entities.EFCore;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Interface.IRepositories;
using ANSH.DDD.Domain.Interface.IUnitOfWorks;
using ANSH.DDD.Domain.Specifications;
using ANSH.DDD.Domain.Specifications.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DDD.Domain.Repository.EFCore {
    /// <summary>
    /// 实体类EFCore仓储实现基类
    /// </summary>
    /// <typeparam name="Entity">实体类</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    /// <typeparam name="TRepositoryUnitOfWork">EFCore仓储操作工作基类接口</typeparam>
    /// <typeparam name="TAggregateRootContxt">EFCore实现</typeparam>
    public abstract class ANSHEFCoreRepositoryBase<Entity, TPKey, TRepositoryUnitOfWork, TAggregateRootContxt> : IANSHEFCoreRepository<Entity, TPKey, TRepositoryUnitOfWork>
        where Entity : ANSHEFCoreEntityBase<TPKey>, new ()
    where TRepositoryUnitOfWork : IANSHEFCoreUnitOfWork, IANSHRepositoryUnitOfWork
    where TPKey : struct, IEquatable<TPKey>
        where TAggregateRootContxt : DBContextOptions<Entity>, new () {

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
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public List<Entity> GetList (IANSHSpecification<Entity> specification = null) => GetListAsync (specification).Result;

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
            public List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20, IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ().Result;

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public async Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public Entity GetOne (IANSHSpecification<Entity> specification = null) => GetOneAsync (specification).Result;

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public Entity GetOne (TPKey Id) {
                var specification = new ANSHEFCoreSpecificationBase<Entity> ();
                specification.SetCriteria (m => m.Id.Equals (Id));
                return GetOne (specification);
            }

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public async Task<Entity> GetOneAsync (TPKey Id) {
                var specification = new ANSHEFCoreSpecificationBase<Entity> ();
                specification.SetCriteria (m => m.Id.Equals (Id));
                return await GetOneAsync (specification);
            }

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public async Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public int Count (IANSHSpecification<Entity> specification = null) => CountAsync (specification).Result;

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public async Task<int> CountAsync (IANSHSpecification<Entity> specification = null) => await Work.Register<TAggregateRootContxt> ().Get (specification).CountAsync ();
        }
}