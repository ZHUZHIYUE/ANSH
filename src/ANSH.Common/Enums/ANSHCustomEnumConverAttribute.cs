using System;

namespace ANSH.Common.Enums {

    /// <summary>
    /// 枚举转换方式
    /// </summary>
    [AttributeUsage (AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ANSHCustomEnumConverAttribute : Attribute {

        /// <summary>
        /// 枚举转换
        /// </summary>
        /// <value></value>
        public ANSHEnumConvertMethod Convert { get; set; } = ANSHEnumConvertMethod.Int;
    }
}