using System;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO {
    /// <summary>
    /// ADO.NET基类
    /// </summary>
    public class ADODBContext {
        /// <summary>
        /// 数据库链接以及事物
        /// </summary>
        public DBConnection server_connection {
            get;
            set;
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        public ILoggerFactory loggerfactory {
            get;
            set;
        }

        ILogger _logger;
        /// <summary>
        /// 日志记录
        /// </summary>
        protected ILogger logger
            => _logger = _logger?? loggerfactory.CreateLogger ($" {this.GetType().Namespace} {this.GetType().Name}");

        /// <summary>
        /// 创建对应的访问层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Set<TResult> ()
        where TResult : ADODBContext, new () {
            return new TResult () { server_connection = server_connection, loggerfactory = loggerfactory };
        }

        /// <summary>
        /// 事物
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="TResultMethod">带有返回值委托</param>
        /// <returns>委托返回值</returns>
        protected TResult ExecuteTransaction<TResult> (Func<TResult> TResultMethod) {
            TResult result = default (TResult);
            try {
                server_connection.BeginTransaction ();
                result = TResultMethod ();
                server_connection.Commit ();
            } catch (Exception ex) {
                server_connection.Rollback ();
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 事物
        /// </summary>
        /// <param name="Method">操作委托</param>
        protected void ExecuteTransaction (Action Method) {
            try {
                server_connection.BeginTransaction ();
                Method ();
                server_connection.Commit ();
            } catch (Exception ex) {
                server_connection.Rollback ();
                throw ex;
            }
        }
    }
}