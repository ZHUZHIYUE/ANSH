using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析GreaterThan运算
    /// </summary>
    public class GreaterThanBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">GreaterThan运算表达式树</param>
        public GreaterThanBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => ">";
    }
}