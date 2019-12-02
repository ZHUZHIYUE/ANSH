using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;
using ANSH.DDD.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// 提供DBContext操作扩展方法
/// </summary>
public static class ANSHDbContextOptionsExtensions {
    /// <summary>
    /// 将指定的查询结果进行分页处理
    /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
    /// </summary>
    /// <typeparam name="TEntity">实体模型</typeparam>
    /// <param name="iqueryable">将指定的查询结果</param>
    /// <param name="dataCount">满足指定条件数据总条数</param>
    /// <param name="pageCount">满足指定条件数据可分页总数</param>
    /// <param name="hasNext">是否还有下一页</param>
    /// <param name="pageIndex">页数</param>
    /// <param name="pageSize">每页数据条数</param>
    /// <returns>返回分页处理后的查询结果</returns>
    public static IQueryable<TEntity> ToPage<TEntity> (this IQueryable<TEntity> iqueryable, out int dataCount, out int pageCount, out bool hasNext, int pageIndex = 1, int pageSize = 20) {
        dataCount = iqueryable.Count ();
        dataCount.ToPage (pageIndex, pageSize, out pageCount, out hasNext);
        return iqueryable.ToTake (pageSize, (pageIndex - 1) * pageSize);
    }

    /// <summary>
    /// 将指定的查询结果取出指定条数
    /// </summary>
    /// <typeparam name="TEntity">实体模型</typeparam>
    /// <param name="iqueryable">将指定的查询结果</param>
    /// <param name="take">取多少条</param>
    /// <param name="skip">忽略前面几个</param>
    /// <returns>返回指定条数的查询结果</returns>
    public static IQueryable<TEntity> ToTake<TEntity> (this IQueryable<TEntity> iqueryable, int take = 20, int skip = 0) {
        return iqueryable.Skip (skip).Take (take);
    }

    /// <summary>
    /// 创建一个 System.Linq.Expressions.Expression，它表示仅在第一个操作数的计算结果为 AND 时才计算第二个操作数的条件true 运算。
    /// </summary>
    /// <typeparam name="T">System.Linq.Expressions.Expression表示的委托的类型</typeparam>
    /// <param name="left">第一个操作</param>
    /// <param name="right">第二个操作</param>
    /// <returns>一个 System.Linq.Expressions.Expression</returns>
    [Obsolete ("已取消")]
    public static Expression<Func<T, bool>> And<T> (this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) {
        if (left == null) {
            return right;
        }
        var param = Expression.Parameter (typeof (T));

        var expr = Expression.AndAlso (Expression.Invoke (left, param), Expression.Invoke (right, param));

        return Expression.Lambda<Func<T, bool>> (expr, param);
    }

    /// <summary>
    /// 创建一个 System.Linq.Expressions.Expression，它表示仅在第一个操作数的计算结果为 OR 时才计算第二个操作数的条件true 运算。
    /// </summary>
    /// <typeparam name="T">System.Linq.Expressions.Expression表示的委托的类型</typeparam>
    /// <param name="left">第一个操作</param>
    /// <param name="right">第二个操作</param>
    /// <returns>一个 System.Linq.Expressions.Expression</returns>
    [Obsolete ("已取消")]
    public static Expression<Func<T, bool>> Or<T> (this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) {
        if (left == null) {
            return right;
        }

        var param = Expression.Parameter (typeof (T));

        var expr = Expression.OrElse (Expression.Invoke (left, param), Expression.Invoke (right, param));

        return Expression.Lambda<Func<T, bool>> (expr, param);
    }

    /// <summary>
    /// 设置规约
    /// </summary>
    /// <typeparam name="TEntity">实体模型</typeparam>
    /// <param name="source">来源</param>
    /// <param name="specification">规约</param>
    public static IQueryable<TEntity> SetSpecification<TEntity> (this IQueryable<TEntity> source, IANSHSpecification<TEntity> specification) where TEntity : class, IANSHDbEntityBase {
        if (specification != null) {
            var Criteria = specification.GetCriteria ();
            source = Criteria.Aggregate (source,
                (current, include) => current.Where (include));

            var InCludes = specification.GetInClude ();
            source = InCludes?.Aggregate (source,
                (current, include) => current.Include (include));

            var ThenInCludes = specification.GetThenInClude ();
            source = ThenInCludes?.Aggregate (source,
                (current, include) => current.Include (include));

            var OrderBy = specification.GetOrderBy ();
            OrderBy?.Reverse ();
            source = OrderBy?.Aggregate (source, (current, include) => include.Value == ANSHSpecificationOrderBy.ASC? current.OrderBy (include.Key) : current.OrderByDescending (include.Key));
        }
        return source;
    }

    /// <summary>
    /// 执行sql
    /// </summary>
    /// <typeparam name="TEntity">模型</typeparam>
    /// <param name="source">来源</param>
    /// <param name="sql">执行的SQL</param>
    /// <param name="anshDbConnection">数据库连接对象</param>
    public static IQueryable<TEntity> FromSql<TEntity> (this DbSet<TEntity> source, FormattableString sql, ANSHDbConnection anshDbConnection) where TEntity : class {
        using (var cmd = anshDbConnection.DbConnection.CreateCommand ()) {
            return source.FromSql (sql);
        }
    }
}