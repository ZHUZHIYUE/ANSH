using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.ADO.Expressions {
    /// <summary>
    /// 解析变量表达式树
    /// </summary>
    public class MemberVisitor : BaseVisitor {

        /// <summary>
        /// 变量表达式树
        /// </summary>
        readonly MemberExpression node;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">变量表达式树</param>
        public MemberVisitor (MemberExpression node) {
            this.node = node;
        }

        /// <summary>
        /// 翻译表达式树
        /// </summary>
        /// <param name="db_parameters">参数</param>
        /// <returns>TSQL</returns>
        public override string Visit (ref List<DBParameters> db_parameters) {
            if (node.Expression.NodeType is ExpressionType.Parameter) {
                return node.Member.Name;
            } else if (node.Expression.NodeType is ExpressionType.Constant) {
                var innerField = (System.Reflection.FieldInfo) node.Member;
                var value = innerField.GetValue (Expression.Constant (node.Expression).Value);
                if (node.Type.IsGenericType) {
                    List<string> items = new List<string> ();
                    var d = node.Type.ReflectedType;
                    foreach (var in_value in (System.Collections.IEnumerable) value) {
                        if (typeof (int).IsAssignableFrom (in_value.GetType ())) {
                            items.Add ($"{(int)in_value}");
                        } else {
                            items.Add ($"'{in_value.ToString()}'");
                        }
                    }
                    return $" {string.Join(",",items.ToArray())} ";
                }
                return value.ToString ();
            } else {
                throw new NotFiniteNumberException ("未知的表达式树。");
            }
        }

        string BIN (MemberExpression ex, ref List<string> p) {
            if (node.Expression.NodeType is ExpressionType.Parameter) {
                return node.Member.Name;
            } else if (node.Expression.NodeType is ExpressionType.Constant) {
                var innerField = (System.Reflection.FieldInfo) node.Member;
                var value = innerField.GetValue (Expression.Constant (node.Expression).Value);
                if (node.Type.IsGenericType) {
                    List<string> items = new List<string> ();
                    var d = node.Type.ReflectedType;
                    foreach (var in_value in (System.Collections.IEnumerable) value) {
                        if (typeof (int).IsAssignableFrom (in_value.GetType ())) {
                            items.Add ($"{(int)in_value}");
                        } else {
                            items.Add ($"'{in_value.ToString()}'");
                        }
                    }
                    return $" {string.Join(",",items.ToArray())} ";
                } else if (p?.Count > 1) {
                    return string.Empty;
                } else {
                    return value.ToString ();
                }
            } else if (node.Expression.NodeType is ExpressionType.MemberAccess) {
                p.Add (node.Member.Name);
                return BIN ((MemberExpression) node.Expression, ref p);
            } else {
                throw new NotFiniteNumberException ("未知的表达式树。");
            }
        }
    }
}