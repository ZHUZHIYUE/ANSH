using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.DataBase.ADO {
    /// <summary>
    /// 数据库主键
    /// </summary>
    [AttributeUsage (AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TBPKAttribute : Attribute {

    }
}