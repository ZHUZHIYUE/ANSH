using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析GreaterThanOrEqual运算
    /// </summary>
    public class GreaterThanOrEqualBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">GreaterThanOrEqual运算表达式树</param>
        public GreaterThanOrEqualBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => ">=";
    }
}