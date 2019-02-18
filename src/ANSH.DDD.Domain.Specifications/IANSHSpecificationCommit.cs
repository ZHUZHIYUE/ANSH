using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ANSH.DDD.Domain.Specifications {
    /// <summary>
    /// 规约接口
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>
    public interface IANSHSpecificationCommit<TEntity> : IANSHSpecification<TEntity> where TEntity : class {

        /// <summary>
        /// 提交保存方式
        /// </summary>
        CommitSaveChangesTypes CommitSaveChanges {
            get;
            set;
        }
    }

    /// <summary>
    /// 提交方式
    /// </summary>
    public enum CommitSaveChangesTypes {
        /// <summary>
        /// 解决乐观并发冲突，用提交数据覆盖数据库数据
        /// </summary>
        Coverage,
        /// <summary>
        /// 解决乐观并发冲突，重新加载数据库中的数据覆盖提交数据
        /// </summary> 
        Reload
    }
}