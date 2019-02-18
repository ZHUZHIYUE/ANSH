using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.DataBase.ADO
{
    /// <summary>
    /// 枚举存取类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TBEnumAttribute : Attribute
    {

        /// <summary>
        /// 存取类型
        /// </summary>
        public enum Access
        {
            /// <summary>
            /// 存取常数名
            /// </summary>
            Name = 1
                ,
            /// <summary>
            /// 存取常数值
            /// </summary>
            Value = 2
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="access">存取类型</param>
        public TBEnumAttribute(Access access = Access.Value)
        {
            CurrentAccess = access;
        }

        /// <summary>
        /// 当前存取类型
        /// </summary>
        public Access CurrentAccess
        {
            get;
            set;
        }
    }
}
