using System;
using System.Collections.Generic;
using System.Data;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// 数据库操作入口
    /// </summary>
    public abstract class DBOptions : IDisposable {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="db_connection">数据库连接</param>
        /// <param name="loggerfactory">日志记录</param>
        public DBOptions (DBConnection db_connection, ILoggerFactory loggerfactory = null) {
            _db_connection = db_connection;
            _loggerfactory = loggerfactory;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        ILoggerFactory _loggerfactory {
            get;
            set;
        }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        string _connection {
            get;
            set;
        }

        /// <summary>
        /// 数据库链接及事物
        /// </summary>
        DBConnection _db_connection {
            get;
            set;
        } = null;

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<DBContext> _dbContext {
            get;
            set;
        } = new List<DBContext> ();

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象都一直保存在集合中，直到集合批量Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Set<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult ();
            result.UseConnection (_db_connection, _loggerfactory);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象不保存在集合中，使用完后需手动Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult SetScope<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult ();
            result.UseConnection (_db_connection, _loggerfactory);
            return result;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (DBContext db) {
            _dbContext.Add (db);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () {
            _dbContext?.ForEach (m => m.Dispose ());
            _db_connection?.Dispose ();
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        public void ExecuteTransaction (Action Method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            try {
                _db_connection.BeginTransaction (isolationLevel);
                _dbContext?.ForEach (m => m.UserTransaction (_db_connection.Transaction));
                Method ();
                _db_connection.Commit ();
            } catch (Exception ex) {
                _db_connection.Rollback ();
                throw ex;
            }
        }
    }
}