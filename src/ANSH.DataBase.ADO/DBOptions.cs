using System;
using System.Collections.Generic;
using System.Data;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO {
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
        /// 日志记录
        /// </summary>
        ILoggerFactory _loggerfactory {
            get;
            set;
        }

        /// <summary>
        /// 创建对应的访问层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Set<TResult> ()
        where TResult : ADODBContext, new () {
            return new TResult () { server_connection = _db_connection, loggerfactory = _loggerfactory };
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () {
            _db_connection?.Dispose ();
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        public void ExecuteTransaction (Action Method, IsolationLevel isolationLevel = IsolationLevel.RepeatableRead) {
            try {
                _db_connection.BeginTransaction (isolationLevel);
                Method ();
                _db_connection.Commit ();
            } catch (Exception ex) {
                _db_connection.Rollback ();
                throw ex;
            }
        }
    }
}