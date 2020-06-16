using System;
using System.Data;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.IUnitOfWorks {
    /// <summary>
    /// 工作单元基类接口
    /// </summary>
    public interface IANSHUnitOfWork : IDisposable {
        /// <summary>
        /// 日志记录
        /// </summary>
        ILoggerFactory Loggerfactory {
            get;
        }

        /// <summary>
        /// 数据库链接及事物
        /// </summary>
        ANSHDbConnection DBconnection {
            get;
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        void ExecuteTransaction (Action Method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        /// <param name="isolationLevel">隔离级别</param>
        TResult ExecuteTransaction<TResult> (Func<TResult> Method, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}