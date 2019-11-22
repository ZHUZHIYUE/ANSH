using ANSH.DataBase.Connection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// SqlServer数据库操作入口
    /// </summary>
    public class ANSHSqlOptionsBase : ANSHDbOptionsBase {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="loggerFactory">日志记录</param>
        public ANSHSqlOptionsBase (string connectionString, ILoggerFactory loggerFactory = null) : base (new ANSHDbConnection (new SqlConnection (connectionString)), loggerFactory) {

        }
    }
}