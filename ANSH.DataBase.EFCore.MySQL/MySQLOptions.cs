using System;
using System.Collections.Generic;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;
using MySql.Data.MySqlClient;

namespace ANSH.DataBase.EFCore.MySQL
{
    /// <summary>
    /// MySQL数据库操作入口
    /// </summary>
    public class MySQLOptions : DBOptions
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        public MySQLOptions(MySqlConnection connection) : base(new DBConnection(connection))
        {

        }
    }
}