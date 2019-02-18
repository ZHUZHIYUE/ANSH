using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析NotEqual运算
    /// </summary>
    public class NotEqualBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">NotEqual运算表达式树</param>
        public NotEqualBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => "<>";
    }
}