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
    /// 泛型实体的抽象实现类，定义泛型实体的公共属性和行为
    /// </summary>
    /// <typeparam name="Entity">泛型实体</typeparam>
    /// <typeparam name="TPKey">主键类型</typeparam>
    public class ANSHQueryBase<Entity, TPKey> : IANSHQuery<Entity, TPKey>
        where Entity : class, IANSHEntity<TPKey>, new ()
    where TPKey : struct, IEquatable<TPKey> {

        /// <summary>
        /// 当前聚合仓储实现
        /// </summary>
        protected IANSHRepository<Entity, TPKey> Repository {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">仓储</param>
        public ANSHQueryBase (IANSHRepository<Entity, TPKey> repository) {
            Repository = repository;
        }

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual List<Entity> GetList (IANSHSpecification<Entity> specification = null) => Repository.GetListAsync (specification).Result;

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
        public virtual List<Entity> GetList (out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20, IANSHSpecification<Entity> specification = null) => Repository.GetList (out datacount, out pagecount, out hasnext, page, pagesize, specification);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual async Task<List<Entity>> GetListAsync (IANSHSpecification<Entity> specification = null) => await Repository.GetListAsync (specification);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual Entity GetOne (IANSHSpecification<Entity> specification = null) => Repository.GetOne (specification);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns>返回满足条件的实体</returns>
        public Entity GetOne (TPKey Id) => Repository.GetOne (Id);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns>返回满足条件的实体</returns>
        public async Task<Entity> GetOneAsync (TPKey Id) => await Repository.GetOneAsync (Id);

        /// <summary>
        /// 获取指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual async Task<Entity> GetOneAsync (IANSHSpecification<Entity> specification = null) => await Repository.GetOneAsync (specification);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        public virtual int Count (IANSHSpecification<Entity> specification = null) => Repository.Count (specification);

        /// <summary>
        /// 获取指定实体数量
        /// </summary>
        /// <param name="specification">规约</param>
        /// <returns>返回满足条件的实体数量</returns>
        public virtual async Task<int> CountAsync (IANSHSpecification<Entity> specification = null) => await Repository.CountAsync (specification);
    }
}