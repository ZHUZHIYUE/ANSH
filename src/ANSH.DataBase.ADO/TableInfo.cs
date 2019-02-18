using System;
using System.Collections.Generic;

namespace ANSH.DataBase.ADO {
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo {

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName {
            get;
            set;
        }

        /// <summary>
        /// 表别名
        /// </summary>
        public string TableASName {
            get;
            set;
        }

        /// <summary>
        /// 表字段
        /// </summary>
        public List<string> Fields {
            get;
            set;
        } = new List<string> ();

        /// <summary>
        /// 表主键
        /// </summary>
        public string PkKey {
            get;
            set;
        }
    }
}