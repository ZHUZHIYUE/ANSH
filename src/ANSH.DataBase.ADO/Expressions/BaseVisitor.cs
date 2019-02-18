using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {

    /// <summary>
    /// 翻译表达式树基类
    /// </summary>
    public abstract class BaseVisitor {

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public abstract string Visit (ref List<DBParameters> db_parameters);

        /// <summary>
        /// 创建翻译表达式树
        /// </summary>
        /// <param name="node">表达式树</param>
        /// <returns>翻译表达式树</returns>
        protected BaseVisitor CreateVisitor (Expression node) {
            switch (node.NodeType) {
                case ExpressionType.Constant:
                    return new ConstantVisitor ((ConstantExpression) node);
                case ExpressionType.Lambda:
                    return new LambdaVisitor ((LambdaExpression) node);
                case ExpressionType.MemberAccess:
                    return new MemberVisitor ((MemberExpression) node);
                case ExpressionType.Convert:
                    return new UnaryVisitor ((UnaryExpression) node);
                case ExpressionType.Call:
                    return new MethodCallVisitor ((MethodCallExpression) node);

                case ExpressionType.Equal:
                    return new EqualBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.AndAlso:
                    return new AndAlsoBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.OrElse:
                    return new OrElseBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.NotEqual:
                    return new NotEqualBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.GreaterThanOrEqual:
                    return new GreaterThanOrEqualBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.GreaterThan:
                    return new GreaterThanBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.LessThanOrEqual:
                    return new LessThanOrEqualBinaryVisitor ((BinaryExpression) node);
                case ExpressionType.LessThan:
                    return new LessThanBinaryVisitor ((BinaryExpression) node);

                default:
                    return default (BaseVisitor);
            }
        }
    }
}