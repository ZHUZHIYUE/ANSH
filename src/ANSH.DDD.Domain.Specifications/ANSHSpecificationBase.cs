using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace ANSH.DDD.Domain.Specifications {
    /// <summary>
    /// 规约实现基类
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>

    public class ANSHSpecificationBase<TEntity> : IANSHSpecification<TEntity> where TEntity : class {

        /// <summary>
        /// 条件筛选
        /// </summary>
        Expression<Func<TEntity, bool>> _Criteria {
            get;
            set;
        }

        /// <summary>
        /// 包含模型集合
        /// </summary>
        List<Expression<Func<TEntity, object>>> _Includes {
            get;
            set;
        } = new List<Expression<Func<TEntity, object>>> ();

        /// <summary>
        /// 包含模型导航集合
        /// </summary>
        List<string> _ThenInClude {
            get;
            set;
        } = new List<string> ();

        /// <summary>
        /// 排序集合
        /// </summary>
        List<KeyValuePair<Expression<Func<TEntity, object>>, ANSHSpecificationOrderBy >> _OrderBy {
            get;
            set;
        } = new List<KeyValuePair<Expression<Func<TEntity, object>>, ANSHSpecificationOrderBy >> ();

        /// <summary>
        /// 条件筛选
        /// </summary>
        /// <param name="criteria">条件筛选</param>
        public void SetCriteria (Expression<Func<TEntity, bool>> criteria) {
            _Criteria = criteria;
        }

        /// <summary>
        /// 获取筛选条件
        /// </summary>
        public Expression<Func<TEntity, bool>> GetCriteria () => _Criteria;

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="includes">包含模型</param>
        public void SetInClude (Expression<Func<TEntity, object>> includes) {
            _Includes.Add (includes);
        }

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="navigationPropertyPath">要包含的分隔导航属性名</param>
        public void SetThenInClude (string navigationPropertyPath) {
            _ThenInClude.Add (navigationPropertyPath);
        }

        /// <summary>
        /// 获取包含模型
        /// </summary>
        public List<Expression<Func<TEntity, object>>> GetInClude () => _Includes;

        /// <summary>
        /// 获取包含模型
        /// </summary>
        public List<string> GetThenInClude () => _ThenInClude;

        /// <summary>
        /// 添加包含模型
        /// </summary>
        /// <param name="includes">包含模型</param>
        /// <param name="order">排序方式</param>
        public void SetOrderBy (Expression<Func<TEntity, object>> includes, ANSHSpecificationOrderBy order) {
            _OrderBy.Add (new KeyValuePair<Expression<Func<TEntity, object>>, ANSHSpecificationOrderBy > (includes, order));
        }

        /// <summary>
        /// 获取排序集合
        /// </summary>
        public List<KeyValuePair<Expression<Func<TEntity, object>>, ANSHSpecificationOrderBy >> GetOrderBy () => _OrderBy;
    }
}