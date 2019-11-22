using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <typeparam name="TAggregateRootContxt">EFCore实现</typeparam>
    public class ANSHEFCoreRepositoryBase<Entity, TPKey, TAggregateRootContxt> : IANSHEFCoreRepository<Entity, TPKey>
        where Entity : ANSHEFCoreEntityBase<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey>
        where TAggregateRootContxt : ANSHDbContextOptionsBase<Entity>, new () {

                /// <summary>
                /// 构造函数
                /// </summary>
                /// <param name="work"></param>
            public ANSHEFCoreRepositoryBase (IANSHEFCoreUnitOfWork work) {
                this.Work = work;
            }

            /// <summary>
            /// EFCore仓储操作工作类
            /// </summary>
            public IANSHEFCoreUnitOfWork Work {
                get;
            }

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector)
            where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector)
            where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).ToListAsync ();

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
            public virtual List<Entity> GetList (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get (specification).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20)
            where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get ().ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get (criteria).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToList ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get ().ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ();

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
            public virtual Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get (specification).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => Work.Register<TAggregateRootContxt> ().Get (criteria).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).ToPage (out datacount, out pagecount, out hasnext, page, pagesize).ToListAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (specification, selector).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (criteria, selector).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (TPKey Id) => Work.Register<TAggregateRootContxt> ().Get (m => m.Id.Equals (Id)).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (m => m.Id.Equals (Id), selector).FirstOrDefault ();

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (TPKey Id) => Work.Register<TAggregateRootContxt> ().Get (m => m.Id.Equals (Id)).FirstOrDefaultAsync ();
            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class => Work.Register<TAggregateRootContxt> ().Get (m => m.Id.Equals (Id), selector).FirstOrDefaultAsync ();

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual int Count (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).Count ();

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual int Count (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).Count ();

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual Task<int> CountAsync (IANSHSpecification<Entity> specification = null) => Work.Register<TAggregateRootContxt> ().Get (specification).CountAsync ();

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual Task<int> CountAsync (Expression<Func<Entity, bool>> criteria) => Work.Register<TAggregateRootContxt> ().Get (criteria).CountAsync ();
        }
}