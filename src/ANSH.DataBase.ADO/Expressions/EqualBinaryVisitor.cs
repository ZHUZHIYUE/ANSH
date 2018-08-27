using System.Linq.Expressions;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析Equal运算
    /// </summary>
    public class EqualBinaryVisitor : BinaryVisitor {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">Equal运算表达式树</param>
        public EqualBinaryVisitor (BinaryExpression node) : base (node) { }

        /// <summary>
        /// 操作符
        /// </summary>
        public override string Method => "=";
    }
}