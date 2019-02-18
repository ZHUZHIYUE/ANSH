using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ANSH.DataBase.ADO;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO.SQLServer {
    /// <summary>
    /// SQLServer操作数据库表
    /// </summary>
    /// <typeparam name="TMODEL">表字段模型</typeparam>
    public class SQLContext<TMODEL> : TBContext<TMODEL>
        where TMODEL : class, new () {
            /// <summary>
            /// 构建INSERT语句
            /// <para>需返回自增列</para>
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="dbparameters">参数</param>
            /// <returns>INSERT语句</returns>
            public override string CreateInsertTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> dbparameters) {
                StringBuilder Tsql = new StringBuilder ();
                if (dbparameters?.Count > 0) {
                    Tsql.Append ($" INSERT INTO {tbinfo.TableName} ({string.Join (",", dbparameters.Keys.ToArray ())}) VALUES ({string.Join (",", dbparameters.Values.ToList().Select(m=>m.ParameterName).ToArray ())});SELECT SCOPE_IDENTITY()");
                }
                return Tsql.ToString ();
            }

            /// <summary>
            /// 构建Update语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="dbparameters">参数</param>
            /// <param name="whereparameters">条件参数</param>
            /// <returns>Update语句</returns>
            public override string CreateUpdateTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> dbparameters, Dictionary<string, DBParameters> whereparameters) {
                StringBuilder Tsql = new StringBuilder ();
                if (dbparameters?.Count > 0) {
                    Tsql.Append ($" UPDATE {tbinfo.TableName} SET ");
                    foreach (var in_dbparameters in dbparameters) {
                        Tsql.Append ($" {in_dbparameters.Key}={in_dbparameters.Value.ParameterName} ,");
                    }
                    Tsql.Remove (Tsql.Length - 1, 1);
                    Tsql.Append (CreateWhere (whereparameters));
                }
                return Tsql.ToString ();
            }

            /// <summary>
            /// 构建Delete语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="whereparameters">条件参数</param>
            /// <returns>Delete语句</returns>
            public override string CreateDeleteTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters) =>
                $" DELETE FROM {tbinfo.TableName} {CreateWhere (whereparameters)}";

            /// <summary>
            /// 构建WHERE语句
            /// </summary>
            /// <param name="whereparameters">条件参数</param>
            /// <param name="TableASName">表别名</param>
            /// <returns>WHERE语句</returns>
            public string CreateWhere (Dictionary<string, DBParameters> whereparameters, string TableASName = null) {
                StringBuilder Tsql = new StringBuilder ();
                List<string> Titem = new List<string> ();
                if (whereparameters?.Count > 0) {
                    Tsql.Append (" WHERE ");
                    foreach (var in_whereparameters in whereparameters) {
                        if (TableASName == null) {
                            Titem.Add ($" {in_whereparameters.Key}={in_whereparameters.Value.ParameterName} ");
                        } else {
                            Titem.Add ($" {TableASName}.{in_whereparameters.Key}={in_whereparameters.Value.ParameterName} ");
                        }
                    }
                    Tsql.Append (string.Join ("AND", Titem.ToArray ()));
                }
                return Tsql.ToString ();
            }

            /// <summary>
            /// 构建Select语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="whereparameters">条件参数</param>
            /// <param name="orderby">排序</param>
            /// <returns>Select语句</returns>
            public override string CreateSelectTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters, params string[] orderby) {
                StringBuilder Tsql = new StringBuilder ();
                Tsql.Append (" SELECT ");
                foreach (var fields in tbinfo.Fields) {
                    Tsql.Append ($" {tbinfo.TableASName}.{fields} ,");
                }
                Tsql.Remove (Tsql.Length - 1, 1);
                Tsql.Append ($" FROM {tbinfo.TableName} {tbinfo.TableASName} {CreateWhere (whereparameters,tbinfo.TableASName)} ");

                if (orderby?.Length > 0) {
                    foreach (var in_orderby in orderby) {
                        Tsql.Append ($" ORDER BY {tbinfo.TableASName}.{in_orderby} ,");
                    }
                    Tsql.Remove (Tsql.Length - 1, 1);
                } else {
                    Tsql.Append ($" ORDER BY {tbinfo.TableASName}.{tbinfo.PkKey}");
                }
                return Tsql.ToString ();
            }

            /// <summary>
            /// 构建Select Count 语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="whereparameters">条件参数</param>
            /// <returns>Select Count 语句</returns>
            public override string CreateSelectCountTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters) => $" SELECT COUNT(*) FROM {tbinfo.TableName} {tbinfo.TableASName} {CreateWhere (whereparameters,tbinfo.TableASName)} ";

            /// <summary>
            /// 构建Page语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="whereparameters">条件参数</param>
            /// <param name="orderby">排序</param>
            /// <param name="output">输出参数</param>
            /// <param name="pageIndex">第几页</param>
            /// <param name="pageSize">每页多少条</param>
            /// <returns>Page语句</returns>
            public override string CreatePageTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters, out Connection.DBParameters output, int pageIndex = 1, int pageSize = 20, params string[] orderby) {
                StringBuilder PageSql = new StringBuilder ();
                StringBuilder Tsql = new StringBuilder ();
                StringBuilder CountSql = new StringBuilder ();
                Tsql.Append (" SELECT ");
                foreach (var fields in tbinfo.Fields) {
                    Tsql.Append ($" {tbinfo.TableASName}.{fields} ,");
                }
                Tsql.Remove (Tsql.Length - 1, 1);
                Tsql.Append ($" FROM {tbinfo.TableName} {tbinfo.TableASName} {CreateWhere (whereparameters,tbinfo.TableASName)} ");

                StringBuilder OrderBysql = new StringBuilder ();
                if (orderby?.Length > 0) {
                    foreach (var in_orderby in orderby) {
                        OrderBysql.Append ($" {in_orderby} ,");
                    }
                    OrderBysql.Remove (OrderBysql.Length - 1, 1);
                } else {
                    OrderBysql.Append ($" {tbinfo.PkKey}");
                }

                return CreatePageTSQL (Tsql.ToString (), OrderBysql.ToString (), out output, pageIndex, pageSize);
            }

            /// <summary>
            /// 构建Page语句
            /// </summary>
            /// <param name="Tsql">查询表SQL</param>
            /// <param name="orderby">排序</param>
            /// <param name="output">输出参数</param>
            /// <param name="pageIndex">第几页</param>
            /// <param name="pageSize">每页多少条</param>
            /// <returns>Page语句</returns>
            public string CreatePageTSQL (string Tsql, string orderby, out DBParameters output, int pageIndex = 1, int pageSize = 20) {
                string CountSql = $" SELECT @ROWSCOUNT=Count(*) FROM ({Tsql}) countpage";
                string PageSql = $"SELECT tbpage.* FROM ( SELECT ROW_NUMBER() OVER (ORDER BY {orderby}) AS tab_Order,* FROM ({Tsql}) AS itmpage ) AS tbpage WHERE tbpage.tab_Order > ${(pageIndex - 1) * pageSize} AND tbpage.tab_Order <= {pageIndex * pageSize};{CountSql.ToString()} ";
                //                 string PageSql = $@"
                // SELECT tbpage.* FROM ({Tsql}) tbpage
                // ORDER BY {orderby}
                // OFFSET {(pageIndex-1)*pageSize} ROWS
                // FETCH NEXT {pageSize} ROWS ONLY ; {CountSql}";
                output = new DBParameters () {
                    ParameterName = "ROWSCOUNT",
                    Size = 32,
                    Direction = System.Data.ParameterDirection.Output,
                    DbType = DbType.Int32
                };
                return PageSql.ToString ();
            }

            /// <summary>
            /// 获取满足条件的模型
            /// <para>分页</para>
            /// </summary>
            /// <param name="TbSql">查询表</param>
            /// <param name="dbparamses">查询表参数</param>
            /// <param name="pageIndex">当前页数</param>
            /// <param name="pageSize">每页显示条数</param>
            /// <param name="datacount">满足指定条件数据总条数</param>
            /// <param name="pagecount">满足指定条件数据可分页总数</param>
            /// <param name="hasnext">是否含有下一页</param>
            /// <param name="orderby">排序</param>
            /// <returns>返回满足条件的模型</returns>
            public virtual List<TMODEL> GetList (string TbSql, List<DBParameters> dbparamses, out int datacount, out int pagecount, out bool hasnext, int pageIndex = 1, int pageSize = 20, params string[] orderby) {
                string Tsql = CreatePageTSQL (TbSql, string.Join (",", orderby?.ToArray ()), out DBParameters output, pageIndex, pageSize);
                using (logger.BeginScope ("GetList TO Page")) {
                    logger?.LogDebug (TbSql);
                    logger?.LogDebug (Tsql);
                }
                dbparamses = dbparamses??new List<DBParameters> ();
                dbparamses.Add (output);
                var result = DataTableToList (base.server_connection.ExecuteSQLQuery (Tsql, dbparamses));
                datacount = Convert.ToInt32 (output.Value);
                pagecount = (int) Math.Ceiling (datacount / (double) pageSize);
                hasnext = pageIndex < pagecount;
                return result;
            }
        }
}