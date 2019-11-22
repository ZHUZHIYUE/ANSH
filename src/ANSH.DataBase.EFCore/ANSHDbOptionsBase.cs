using System;
using System.Collections.Generic;
using System.Data;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// 数据库操作入口
    /// </summary>
    public abstract class ANSHDbOptionsBase : IDisposable {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="anshDbConnection">数据库连接</param>
        /// <param name="loggerFactory">日志记录</param>
        public ANSHDbOptionsBase (ANSHDbConnection anshDbConnection, ILoggerFactory loggerFactory = null) {
            _ANSHDbConnection = anshDbConnection;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        ILoggerFactory _loggerFactory {
            get;
            set;
        }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        string _ConnectionString {
            get;
            set;
        }

        /// <summary>
        /// 数据库链接及事物
        /// </summary>
        ANSHDbConnection _ANSHDbConnection {
            get;
            set;
        } = null;

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<ANSHDbContextBase> _ANSHDbContext {
            get;
            set;
        } = new List<ANSHDbContextBase> ();

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象都一直保存在集合中，直到集合批量Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Register<TResult> ()
        where TResult : ANSHDbContextBase, new () {
            var result = new TResult ();
            result.UseConnection (_ANSHDbConnection, _loggerFactory);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象不保存在集合中，使用完后需手动Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult RegisterScope<TResult> ()
        where TResult : ANSHDbContextBase, new () {
            var result = new TResult ();
            result.UseConnection (_ANSHDbConnection, _loggerFactory);
            return result;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="anshDbContext"></param>
        void AddDbContext (ANSHDbContextBase anshDbContext) {
            _ANSHDbContext.Add (anshDbContext);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () {
            _ANSHDbContext?.ForEach (m => m.Dispose ());
            _ANSHDbConnection?.Dispose ();
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        public void ExecuteTransaction (Action method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            try {
                _ANSHDbConnection.BeginTransaction (isolationLevel);
                _ANSHDbContext?.ForEach (m => m.UserTransaction (_ANSHDbConnection.DbTransaction));
                method ();
                _ANSHDbConnection.Commit ();
            } catch (Exception ex) {
                _ANSHDbConnection.Rollback ();
                throw ex;
            }
        }
    }
}