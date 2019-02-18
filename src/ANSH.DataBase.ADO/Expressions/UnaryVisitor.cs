using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析一元运算符表达式树
    /// </summary>
    public class UnaryVisitor : BaseVisitor {

        /// <summary>
        /// 一元运算符表达式树
        /// </summary>
        readonly UnaryExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">一元运算符表达式树</param>
        public UnaryVisitor (UnaryExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            return CreateVisitor (node.Operand).Visit (ref db_parameters);
        }
    }
}