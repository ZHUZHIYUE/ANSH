using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ANSH.DDD.Domain.Interface.IEntities;
using ANSH.DDD.Domain.Specifications;

namespace ANSH.DDD.Domain.Interface.IRepositories {

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="Entity">领域实体</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public interface IANSHRepository<Entity, TPKey> where Entity : class, IANSHEntity<TPKey> where TPKey : struct, IEquatable<TPKey> {

        #region GetList
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (Expression<Func<Entity, bool>> criteria);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification = null);
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector)
        where TResult : class;
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector)
        where TResult : class;
        #endregion

        #region GetListToPage
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
        List<Entity> GetList (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20)
        where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="datacount">满足指定条件数据总条数</param>
        /// <param name="pagecount">满足指定条件数据可分页总数</param>
        /// <param name="hasnext">是否还有下一页</param>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数据条数</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        List<Entity> GetList (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="datacount">满足指定条件数据总条数</param>
        /// <param name="pagecount">满足指定条件数据可分页总数</param>
        /// <param name="hasnext">是否还有下一页</param>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数据条数</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class;

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
        Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20);

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
        Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TResult : class;
        #endregion

        #region GetListToTake
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (IANSHSpecification<Entity> specification, int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        List<TResult> GetList<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, int take, int skip = 0)
        where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (Expression<Func<Entity, bool>> criteria, int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        List<TResult> GetList<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, int take, int skip = 0) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification, int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<TResult>> GetListAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector, int take, int skip = 0) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<Entity>> GetListAsync (Expression<Func<Entity, bool>> criteria, int take, int skip = 0);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <param name="take">取多少条</param>
        /// <param name="skip">忽略前面几个</param>
        /// <returns>返回满足条件的实体</returns>
        Task<List<TResult>> GetListAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector, int take, int skip = 0) where TResult : class;
        #endregion

        #region GetOne
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        Entity GetOne (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体
        /// /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        TResult GetOne<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体</returns>
        Entity GetOne (Expression<Func<Entity, bool>> criteria);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        TResult GetOne<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        Task<TResult> GetOneAsync<TResult> (IANSHSpecification<Entity> specification, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体</returns>
        Task<Entity> GetOneAsync (Expression<Func<Entity, bool>> criteria);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        Task<TResult> GetOneAsync<TResult> (Expression<Func<Entity, bool>> criteria, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns>返回满足条件的实体</returns>
        Entity GetOne (TPKey Id);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        TResult GetOne<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class;

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns>返回满足条件的实体</returns>
        Task<Entity> GetOneAsync (TPKey Id);
        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        Task<TResult> GetOneAsync<TResult> (TPKey Id, Expression<Func<Entity, TResult>> selector) where TResult : class;
        #endregion

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        int Count (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体数量</returns>
        int Count (Expression<Func<Entity, bool>> criteria);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        Task<int> CountAsync (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="criteria">条件</param>
        /// <returns>返回满足条件的实体数量</returns>
        Task<int> CountAsync (Expression<Func<Entity, bool>> criteria);
    }
}