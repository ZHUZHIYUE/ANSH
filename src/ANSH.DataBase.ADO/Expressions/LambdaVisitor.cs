using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析Lambda表达式树
    /// </summary>
    public class LambdaVisitor : BaseVisitor {

        /// <summary>
        /// Lambda表达式树
        /// </summary>
        readonly LambdaExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">Lambda表达式树</param>
        public LambdaVisitor (LambdaExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            return CreateVisitor (node.Body).Visit (ref db_parameters);
        }
    }
}