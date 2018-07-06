using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// SqlServer数据库操作入口
    /// </summary>
    public class SQLOptions : DBOptions {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        public SQLOptions (string connection) : base (new DBConnection (new SqlConnection (connection))) {

        }
    }
}