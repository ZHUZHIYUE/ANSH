using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析静态方法或实例方法表达式树
    /// </summary>
    public class MethodCallVisitor : BaseVisitor {

        /// <summary>
        /// 静态方法或实例方法表达式树
        /// </summary>
        readonly MethodCallExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">静态方法或实例方法表达式树</param>
        public MethodCallVisitor (MethodCallExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            var methodInfo = node.Method;
            if (methodInfo.Name == "Contains") {
                var receiverVisitor = CreateVisitor (node.Object);
                var value = receiverVisitor.Visit (ref db_parameters);
                return $" {CreateVisitor (node.Arguments[0]).Visit(ref db_parameters)} IN ( {value} ) ";
            } else {
                throw new NotFiniteNumberException ("未知的操作方法。");
            }
        }
    }
}