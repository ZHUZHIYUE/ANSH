using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.DataBase.ADO {
    /// <summary>
    /// 数据库表属性
    /// </summary>
    [AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute {

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName {
            get;
            set;
        }

        /// <summary>
        /// 表别名
        /// </summary>
        public string ASName {
            get;
            set;
        } = "tb";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="asname">别名</param>
        public TableAttribute (string tablename, string asname = "tb") { TableName = tablename; ASName = asname; }
    }
}