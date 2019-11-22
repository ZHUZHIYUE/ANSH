using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.IUnitOfWorks.EFCore {

    /// <summary>
    /// EFCore仓储操作工作基类
    /// </summary>
    public abstract class ANSHEFCoreUnitOfWorkBase : ANSHUnitOfWorkBase, IANSHEFCoreUnitOfWork {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="db_connection">数据库连接</param>
        /// <param name="loggerfactory">日志记录</param>
        public ANSHEFCoreUnitOfWorkBase (ANSHDbConnection db_connection, ILoggerFactory loggerfactory = null) : base (db_connection, loggerfactory) { }

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<ANSHDbContextBase> _ANSHDbContextBase = new List<ANSHDbContextBase> ();

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象都一直保存在集合中，直到集合批量Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Register<TResult> ()
        where TResult : ANSHDbContextBase, new () {
            var result = new TResult ();
            result.UseConnection ((IsBeginTransactionThreadLocal.IsValueCreated && IsBeginTransactionThreadLocal.Value) ? this.TransactionDBConnectionThreadLocal.Value : base.DBconnection, base.Loggerfactory);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (ANSHDbContextBase db) {
            _ANSHDbContextBase.Add (db);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose () {
            base.Dispose ();
            _ANSHDbContextBase?.ForEach (m => m.Dispose ());
            _ANSHDbContextBase?.Clear ();
            ClearTransactionDBConnectionThreadLocal ();
        }

        /// <summary>
        /// 事物新建DBConnection对象
        /// </summary>
        ThreadLocal<ANSHDbConnection> TransactionDBConnectionThreadLocal { get; set; } = new ThreadLocal<ANSHDbConnection> ();

        /// <summary>
        /// 创建Connection对象
        /// </summary>
        /// <returns>Connection对象</returns>
        protected abstract ANSHDbConnection CreateDBConnection ();

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        public override void ExecuteTransaction (Action Method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            try {
                ++BeginTransactionCount.Value;
                TransactionDBConnectionThreadLocal.Value = TransactionDBConnectionThreadLocal.Value??CreateDBConnection ();
                TransactionDBConnectionThreadLocal.Value.BeginTransaction (isolationLevel);
                IsBeginTransactionThreadLocal.Value = true;
                Method ();
                TransactionDBConnectionThreadLocal.Value.Commit ();
                --BeginTransactionCount.Value;
                if (BeginTransactionCount.Value == 0) { ClearTransactionDBConnectionThreadLocal (); }
            } catch (Exception ex) {
                ClearTransactionDBConnectionThreadLocal ();
                throw ex;
            }
        }

        /// <summary>
        /// 清除事物DBConnection对象
        /// </summary>
        void ClearTransactionDBConnectionThreadLocal () {
            TransactionDBConnectionThreadLocal.Value?.Rollback ();
            TransactionDBConnectionThreadLocal.Value?.Dispose ();
            TransactionDBConnectionThreadLocal.Value = null;
            IsBeginTransactionThreadLocal.Value = false;
            BeginTransactionCount.Value = 0;
        }

        /// <summary>
        /// 事物计数
        /// </summary>
        ThreadLocal<int> BeginTransactionCount { get; set; } = new ThreadLocal<int> (() => 0);
    }
}