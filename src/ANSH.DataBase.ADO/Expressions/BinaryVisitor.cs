using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析二进制运算符表达式树
    /// </summary>
    public abstract class BinaryVisitor : BaseVisitor {

        /// <summary>
        /// 二进制运算符表达式树
        /// </summary>
        readonly BinaryExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">二进制运算符表达式树</param>
        public BinaryVisitor (BinaryExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            var left = CreateVisitor (node.Left);
            var right = CreateVisitor (node.Right);
            return $" {left.Visit (ref db_parameters)} {Method} {right.Visit (ref db_parameters)} ";
        }

        /// <summary>
        /// 操作符
        /// </summary>
        public abstract string Method {
            get;
        }
    }
}