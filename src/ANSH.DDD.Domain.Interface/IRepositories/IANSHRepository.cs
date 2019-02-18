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
        /// <param name="datacount">满足指定条件数据总条数</param>
        /// <param name="pagecount">满足指定条件数据可分页总数</param>
        /// <param name="hasnext">是否还有下一页</param>
        /// <param name="page">页数</param>
        /// <param name="pagesize">每页数据条数</param>
        /// <returns>返回满足条件的实体</returns>
        List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20, IANSHSpecification<Entity> specification = null);

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
        /// <returns>返回满足条件的实体</returns>
        Entity GetOne (IANSHSpecification<Entity> specification = null);

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
        /// <returns>返回满足条件的实体</returns>
        Task<Entity> GetOneAsync (TPKey Id);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        int Count (IANSHSpecification<Entity> specification = null);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        Task<int> CountAsync (IANSHSpecification<Entity> specification = null);
    }
}