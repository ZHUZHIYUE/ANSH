using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析AndAlso运算
    /// </summary>
    public class AndAlsoBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">AndAlso运算表达式树</param>
        public AndAlsoBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            return $" ( {base.Visit(ref db_parameters)} ) ";
        }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => "AND";
    }
}