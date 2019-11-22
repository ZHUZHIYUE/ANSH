using System;
using System.Data;
using System.Data.Common;
namespace ANSH.DataBase.Connection {
    /// <summary>
    /// 参数
    /// </summary>
    public class ANSHDbParameter {
        /// <summary>
        /// 参数名称
        /// </summary>
        /// <returns></returns>
        public string ParameterName {
            get;
            set;
        }

        /// <summary>
        /// 参数值
        /// </summary>
        /// <returns></returns>
        public object Value {
            get;
            set;
        }

        /// <summary>
        /// 参数大小
        /// </summary>
        /// <returns></returns>
        public int? Size {
            get;
            set;
        }

        /// <summary>
        /// 参数属性
        /// </summary>
        /// <returns></returns>
        public ParameterDirection Direction {
            get;
            set;
        } = ParameterDirection.Input;

        /// <summary>
        /// 参数类型
        /// </summary>
        public DbType? DbType {
            get;
            set;
        }
    }
}