using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace ANSH.DDD.Domain.Specifications {
    /// <summary>
    /// 规约实现基类
    /// </summary>
    /// <typeparam name="TEntity">规约模型</typeparam>
    public class ANSHSpecificationCommitBase<TEntity> : ANSHSpecificationBase<TEntity>, IANSHSpecificationCommit<TEntity> where TEntity : class {

        /// <summary>
        /// 提交保存方式
        /// </summary>
        public CommitSaveChangesTypes CommitSaveChanges {
            get;
            set;
        }
    }
}