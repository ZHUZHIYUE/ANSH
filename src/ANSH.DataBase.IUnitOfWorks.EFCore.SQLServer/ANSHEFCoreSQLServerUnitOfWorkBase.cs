using System;
using System.Data;
using System.Threading;
using ANSH.DataBase.Connection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.IUnitOfWorks.EFCore.SQLServer {

     /// <summary>
     /// EFCoreSQLServer仓储操作工作基类
     /// </summary>
     public class ANSHEFCoreSQLServerUnitOfWorkBase : ANSHEFCoreUnitOfWorkBase, IANSHEFCoreSQLServerUnitOfWork {

          /// <summary>
          /// 创建数据库连接
          /// </summary>
          /// <param name="ConnectionString">数据库连接字符串</param>
          /// <param name="loggerfactory">日志记录</param>
          public ANSHEFCoreSQLServerUnitOfWorkBase (string ConnectionString, ILoggerFactory loggerfactory = null) : base (new ANSHDbConnection (new SqlConnection (ConnectionString)), loggerfactory) {
               this.ConnectionString = ConnectionString;
          }

          /// <summary>
          /// 数据库连接字符串
          /// </summary>
          string ConnectionString {
               get;
               set;
          }

          /// <summary>
          /// 创建Connection对象
          /// </summary>
          /// <returns>Connection对象</returns>
          protected override ANSHDbConnection CreateDBConnection () => new ANSHDbConnection (new SqlConnection (this.ConnectionString));
     }
}