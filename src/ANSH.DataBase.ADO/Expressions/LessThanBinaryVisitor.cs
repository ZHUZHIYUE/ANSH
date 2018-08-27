using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析LessThan运算
    /// </summary>
    public class LessThanBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">LessThan运算表达式树</param>
        public LessThanBinaryVisitor (BinaryExpression node) : base (node) { 
        }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => "<";
    }
}