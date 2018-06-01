using System;
using System.Linq;
using System.Linq.Expressions;
using ANSH.DataBase.EFCore;

/// <summary>
/// 提供DBContext操作扩展方法
/// </summary>
public static class DBContextOptionsExtensions {
    /// <summary>
    /// 将指定的查询结果进行分页处理
    /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
    /// </summary>
    /// <param name="iqueryable">将指定的查询结果</param>
    /// <param name="datacount">满足指定条件数据总条数</param>
    /// <param name="pagecount">满足指定条件数据可分页总数</param>
    /// <param name="hasnext">是否还有下一页</param>
    /// <param name="page">页数</param>
    /// <param name="pagesize">每页数据条数</param>
    /// <returns>返回分页处理后的查询结果</returns>
    public static IQueryable<TEntity> ToPage<TEntity> (this IQueryable<TEntity> iqueryable, out int datacount, out int pagecount, out bool hasnext, int page = 1, int pagesize = 20) where TEntity : DBEntity {
        datacount = iqueryable.Count ();
        pagecount = (int) Math.Ceiling (datacount / (double) pagesize);
        hasnext = page < pagecount;
        return iqueryable.Skip ((page - 1) * pagesize).Take (pagesize);
    }

    /// <summary>
    /// 创建一个 System.Linq.Expressions.Expression，它表示仅在第一个操作数的计算结果为 AND 时才计算第二个操作数的条件true 运算。
    /// </summary>
    /// <typeparam name="T">System.Linq.Expressions.Expression表示的委托的类型</typeparam>
    /// <param name="left">第一个操作</param>
    /// <param name="right">第二个操作</param>
    /// <returns>一个 System.Linq.Expressions.Expression</returns>
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
    public static Expression<Func<T, bool>> Or<T> (this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) {
        if (left == null) {
            return right;
        }

        var param = Expression.Parameter (typeof (T));

        var expr = Expression.OrElse (Expression.Invoke (left, param), Expression.Invoke (right, param));

        return Expression.Lambda<Func<T, bool>> (expr, param);
    }
}