using System;
using System.Collections.Generic;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ANSH.DataBase.EFCore.MySQL {
    /// <summary>
    /// MySQL数据库操作入口
    /// </summary>
    public class ANSHMySqlOptionsBase : ANSHDbOptionsBase {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接</param>
        /// <param name="loggerFactory">日志记录</param>
        public ANSHMySqlOptionsBase (string connectionString,ILoggerFactory loggerFactory = null) : base (new ANSHDbConnection (new MySqlConnection (connectionString)),loggerFactory) {

        }
    }
}