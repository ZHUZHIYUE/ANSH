using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析常量表达式树
    /// </summary>
    public class ConstantVisitor : BaseVisitor {

        /// <summary>
        /// 常量表达式树
        /// </summary>
        readonly ConstantExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">常量表达式树</param>
        public ConstantVisitor (ConstantExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            return node.Value.ToString ();
        }
    }
}