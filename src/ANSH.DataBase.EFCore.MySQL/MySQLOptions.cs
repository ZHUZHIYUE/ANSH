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
    public class MySQLOptions : DBOptions {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="loggerfactory">日志记录</param>
        public MySQLOptions (string connection,ILoggerFactory loggerfactory = null) : base (new DBConnection (new MySqlConnection (connection)),loggerfactory) {

        }
    }
}