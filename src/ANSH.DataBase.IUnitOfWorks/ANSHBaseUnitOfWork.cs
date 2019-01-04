using System;
using System.Data;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.IUnitOfWorks {
    /// <summary>
    /// 工作单元基类实现
    /// </summary>
    public class ANSHBaseUnitOfWork : IANSHUnitOfWork {

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="db_connection">数据库连接</param>
        /// <param name="loggerfactory">日志记录</param>
        public ANSHBaseUnitOfWork (DBConnection db_connection, ILoggerFactory loggerfactory = null) {
            DBconnection = db_connection;
            Loggerfactory = loggerfactory;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        public ILoggerFactory Loggerfactory {
            get;
        }

        /// <summary>
        /// 数据库链接及事物
        /// </summary>
        public DBConnection DBconnection {
            get;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose () {
            DBconnection?.Dispose ();
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        public void ExecuteTransaction (Action Method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            try {
                DBconnection.BeginTransaction (isolationLevel);
                Method ();
                DBconnection.Commit ();
            } catch (Exception ex) {
                DBconnection.Rollback ();
                throw ex;
            }
        }
    }
}