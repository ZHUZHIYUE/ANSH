using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析LessThanOrEqual运算
    /// </summary>
    public class LessThanOrEqualBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">LessThanOrEqual运算表达式树</param>
        public LessThanOrEqualBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => "<=";
    }
}