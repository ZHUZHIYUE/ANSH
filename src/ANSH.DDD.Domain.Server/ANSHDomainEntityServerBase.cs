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
    /// 实体EFCore仓储服务
    /// </summary>
    /// <typeparam name="TIANSHRepository">聚合仓储实现</typeparam>
    /// <typeparam name="Entity">领域实体</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public abstract class ANSHDomainEntityServerBase<TIANSHRepository, Entity, TPKey> : ANSHDomainServerBase, IANSHDomainEntityServer<TIANSHRepository, Entity, TPKey>
        where TIANSHRepository : IANSHRepository<Entity, TPKey>
        where Entity : class, IANSHEntity<TPKey>
        where TPKey : struct, IEquatable<TPKey> {

            /// <summary>
            /// 聚合根仓储实现
            /// </summary>
            public abstract TIANSHRepository IRepository {
                get;
            }

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (IANSHSpecification<Entity> specification = null) => IRepository.GetList (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetList (specification, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (Expression<Func<Entity, bool>> criteria) => IRepository.GetList (criteria);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetList (criteria, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification = null) => IRepository.GetListAsync (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector)
            where TResult : class => IRepository.GetListAsync (specification, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria) => IRepository.GetListAsync (criteria);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector)
            where TResult : class => IRepository.GetListAsync (criteria, selector);

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
            public virtual List<Entity> GetList (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetList (specification, out datacount, out pagecount, out hasnext, page, pagesize);

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
            where TResult : class => IRepository.GetList (specification, selector, out datacount, out pagecount, out hasnext, page, pagesize);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetList (out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual List<Entity> GetList (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetList (criteria, out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => IRepository.GetList (criteria, selector, out datacount, out pagecount, out hasnext, page, pagesize);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否还有下一页</param>
            /// <param name="page">页数</param>
            /// <param name="pagesize">每页数据条数</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<List<Entity>> GetListAsync (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetListAsync (out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetListAsync (specification, out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => IRepository.GetListAsync (specification, selector, out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) => IRepository.GetListAsync (criteria, out datacount, out pagecount, out hasnext, page, pagesize);

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
            public virtual Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class => IRepository.GetListAsync (criteria, selector, out datacount, out pagecount, out hasnext, page, pagesize);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (IANSHSpecification<Entity> specification = null) => IRepository.GetOne (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOne (specification, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (Expression<Func<Entity, bool>> criteria) => IRepository.GetOne (criteria);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOne (criteria, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null) => IRepository.GetOneAsync (specification);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="specification">规约</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOneAsync (specification, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (Expression<Func<Entity, bool>> criteria) => IRepository.GetOneAsync (criteria);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOneAsync (criteria, selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Entity GetOne (TPKey Id) => IRepository.GetOne (m => m.Id.Equals (Id));

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual TResult GetOne<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOne (m => m.Id.Equals (Id), selector);

            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<Entity> GetOneAsync (TPKey Id) => IRepository.GetOneAsync (m => m.Id.Equals (Id));
            /// <summary>
            /// 获取指定实体
            /// </summary>
            /// <param name="Id">主键</param>
            /// <param name="selector">返回指定实体类型</param>
            /// <returns>返回满足条件的实体</returns>
            public virtual Task<TResult> GetOneAsync<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class => IRepository.GetOneAsync (m => m.Id.Equals (Id), selector);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual int Count (IANSHSpecification<Entity> specification = null) => IRepository.Count (specification);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual int Count (Expression<Func<Entity, bool>> criteria) => IRepository.Count (criteria);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="specification">规约</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual Task<int> CountAsync (IANSHSpecification<Entity> specification = null) => IRepository.CountAsync (specification);

            /// <summary>
            /// 获取指定实体数量
            /// </summary>
            /// <param name="criteria">条件</param>
            /// <returns>返回满足条件的实体数量</returns>
            public virtual Task<int> CountAsync (Expression<Func<Entity, bool>> criteria) => IRepository.CountAsync (criteria);
        }
}