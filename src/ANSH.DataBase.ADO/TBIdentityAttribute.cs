using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANSH.DataBase.ADO
{
    /// <summary>
    /// 数据库表自增列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class TBIdentityAttribute : Attribute
    {

    }
}
