using System;
using System.Collections.Generic;
using System.Linq;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO {
    /// <summary>
    /// 操作数据库表基类
    /// </summary>
    /// <typeparam name="TMODEL">表字段模型</typeparam>
    public abstract class TBContext<TMODEL> : VIEWContext<TMODEL>
        where TMODEL : class, new () {
            /// <summary>
            /// 添加一条数据
            /// </summary>
            /// <param name="insert">需要添加的数据</param>
            /// <returns>返回自增列</returns>
            public virtual int? Insert (Action<TMODEL> insert) {
                var model = new TMODEL ();
                insert?.Invoke (model);
                return Insert (model);
            }

            /// <summary>
            /// 添加一条数据
            /// </summary>
            /// <param name="model">需要添加的数据</param>
            /// <returns>返回自增列</returns>
            public virtual int? Insert (TMODEL model) {
                var db_insert = ConvertMODEL (model, out TableInfo tbinfo);
                string sql_insert = CreateInsertTSQL (tbinfo, db_insert);
                if (string.IsNullOrWhiteSpace (sql_insert)) {
                    return null;
                }
                logger?.LogDebug (sql_insert);
                return int.TryParse (base.server_connection.ExecuteSQLScalar (sql_insert, db_insert.Values.ToList ())?.ToString (), out int identity) ? (int?) identity : null;
            }

            /// <summary>
            /// 修改指定数据
            /// </summary>
            /// <param name="update">新的数据</param>
            /// <param name="action">查询条件</param>
            public virtual void Update (Action<TMODEL> update, Action<TMODEL> action) {
                var model = new TMODEL ();
                update?.Invoke (model);
                var wheres = new TMODEL ();
                action?.Invoke (wheres);
                var db_update = ConvertMODEL (model, out TableInfo tbinfo);
                var db_where = ConvertWHERE (wheres, out TableInfo _);
                string sql_update = CreateUpdateTSQL (tbinfo, db_update, db_where);
                if (string.IsNullOrWhiteSpace (sql_update)) {
                    return;
                }
                logger?.LogDebug (sql_update);
                List<DBParameters> dbparamses = new List<DBParameters> ();
                dbparamses.AddRange (db_update.Values.ToList ());
                dbparamses.AddRange (db_where.Values.ToList ());
                dbparamses.Remove (null);
                base.server_connection.ExecuteSQLNonQuery (sql_update, dbparamses);
            }

            /// <summary>
            /// 删除指定数据
            /// </summary>
            /// <param name="action">查询条件</param>
            public virtual void Delete (Action<TMODEL> action) {
                var wheres = new TMODEL ();
                action?.Invoke (wheres);
                var db_where = ConvertWHERE (wheres, out TableInfo tbinfo);
                string sql_delete = CreateDeleteTSQL (tbinfo, db_where);
                if (string.IsNullOrWhiteSpace (sql_delete)) {
                    return;
                }
                logger?.LogDebug (sql_delete);
                base.server_connection.ExecuteSQLNonQuery (sql_delete, db_where.Values.ToList ());
            }

            /// <summary>
            /// 构建INSERT语句
            /// <para>需返回自增列</para>
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="dbparameters">参数</param>
            /// <returns>INSERT语句</returns>
            public abstract string CreateInsertTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> dbparameters);

            /// <summary>
            /// 构建Update语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="dbparameters">参数</param>
            /// <param name="whereparameters">条件参数</param>
            /// <returns>Update语句</returns>
            public abstract string CreateUpdateTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> dbparameters, Dictionary<string, DBParameters> whereparameters);

            /// <summary>
            /// 构建Delete语句
            /// </summary>
            /// <param name="tbinfo">表信息</param>
            /// <param name="whereparameters">条件参数</param>
            /// <returns>Delete语句</returns>
            public abstract string CreateDeleteTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters);
        }
}