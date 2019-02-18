using System;
using System.Data;
using System.Data.SqlClient;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO.SQLServer {
    /// <summary>
    /// SqlServer数据库操作入口
    /// </summary>
    public class SQLOptions : DBOptions {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="logger">日志记录</param>
        public SQLOptions (string connection, ILoggerFactory logger = null) : base (new DBConnection (new SqlConnection (connection)), logger) {

        }
    }
}