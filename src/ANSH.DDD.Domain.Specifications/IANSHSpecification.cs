using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ANSH.DDD.Domain.Specifications {
    /// <summary>
    /// 规约接口
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>
    public interface IANSHSpecification<TEntity> where TEntity : class {

        /// <summary>
        /// 条件筛选
        /// </summary>
        /// <param name="criteria">条件筛选</param>
        void SetCriteria (Expression<Func<TEntity, bool>> criteria);

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        List<Expression<Func<TEntity, bool>>> GetCriteria ();

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="includes">包含模型</param>
        void SetInClude (Expression<Func<TEntity, object>> includes);

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="navigationPropertyPath">要包含的分隔导航属性名</param>
        void SetThenInClude (string navigationPropertyPath);

        /// <summary>
        /// 获取包含模型
        /// </summary>
        List<Expression<Func<TEntity, object>>> GetInClude ();

        /// <summary>
        /// 获取包含模型
        /// </summary>
        List<string> GetThenInClude ();

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="includes">包含模型</param>
        /// <param name="order">排序方式</param>
        void SetOrderBy (Expression<Func<TEntity, object>> includes, ANSHSpecificationOrderBy order);

        /// <summary>
        /// 获取排序集合
        /// </summary>
        List<KeyValuePair<Expression<Func<TEntity, object>>, ANSHSpecificationOrderBy >> GetOrderBy ();
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum ANSHSpecificationOrderBy {
        /// <summary>
        /// 升序
        /// </summary>
        ASC,
        /// <summary>
        /// 降序
        /// </summary>
        DESC
    }
}