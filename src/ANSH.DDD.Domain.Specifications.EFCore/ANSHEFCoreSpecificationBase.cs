using System;
using System.Linq.Expressions;
using ANSH.DataBase.EFCore;

namespace ANSH.DDD.Domain.Specifications.EFCore {
    /// <summary>
    /// EFCore规约实现基类
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>
    public class ANSHEFCoreSpecificationBase<TEntity> : ANSHSpecificationCommitBase<TEntity>, IANSHEFCoreSpecificationCommit<TEntity> where TEntity : class, IANSHDbEntityBase {
    }
}