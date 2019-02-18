using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using ANSH.DataBase.Connection;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.ADO {

    /// <summary>
    /// 操作数据库视图基类
    /// </summary>
    /// <typeparam name="TMODEL">表字段模型</typeparam>
    public abstract class VIEWContext<TMODEL> : ADODBContext
    where TMODEL : class, new () {
        /// <summary>
        /// 获取满足条件的数量
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>返回满足条件的数量</returns>
        public virtual int GetCount (Action<TMODEL> where) {
            var Twheres = new TMODEL ();
            where?.Invoke (Twheres);
            var db_where = ConvertWHERE (Twheres, out TableInfo tbinfo);
            string Tsql = CreateSelectCountTSQL (tbinfo, db_where);
            logger?.LogDebug (Tsql);
            return (int) base.server_connection.ExecuteSQLScalar (Tsql.ToString (), db_where.Values.ToList ());
        }

        /// <summary>
        /// 获取满足条件的第一个模型
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>返回满足条件的第一个模型</returns>
        public virtual TMODEL GetModel (Action<TMODEL> where) {
            var result = GetList (where);
            if (result?.Count > 0) {
                return result[0];
            }
            return null;
        }

        /// <summary>
        /// 获取满足条件的第一个模型
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>返回满足条件的第一个模型</returns>
        public virtual TMODEL GetModel (TMODEL where) {
            var result = GetList (where);
            if (result?.Count > 0) {
                return result[0];
            }
            return null;
        }

        /// <summary>
        /// 获取满足条件的所有模型
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <returns>返回满足条件的所有模型</returns>
        public virtual List<TMODEL> GetList (Action<TMODEL> where, params string[] orderby) {
            var Twheres = new TMODEL ();
            where?.Invoke (Twheres);
            return GetList (Twheres, orderby);
        }

        /// <summary>
        /// 获取满足条件的所有模型
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <returns>返回满足条件的所有模型</returns>
        public virtual List<TMODEL> GetList (TMODEL where, params string[] orderby) {
            var db_where = ConvertWHERE (where, out TableInfo tbinfo);
            string Tsql = CreateSelectTSQL (tbinfo, db_where, orderby);
            logger?.LogDebug (Tsql);
            return DataTableToList (base.server_connection.ExecuteSQLQuery (Tsql.ToString (), db_where.Values.ToList ()));
        }

        /// <summary>
        /// 获取满足条件的模型
        /// <para>分页</para>
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="datacount">满足指定条件数据总条数</param>
        /// <param name="pagecount">满足指定条件数据可分页总数</param>
        /// <param name="hasnext">是否含有下一页</param>
        /// <param name="orderby">排序</param>
        /// <returns>返回满足条件的模型</returns>
        public virtual List<TMODEL> GetList (out int datacount, out int pagecount, out bool hasnext, Action<TMODEL> where, int pageIndex = 1, int pageSize = 20, params string[] orderby) {
            var Twheres = new TMODEL ();
            where?.Invoke (Twheres);
            return GetList (out datacount, out pagecount, out hasnext, Twheres, pageIndex, pageSize, orderby);
        }

        /// <summary>
        /// 获取满足条件的模型
        /// <para>分页</para>
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="datacount">满足指定条件数据总条数</param>
        /// <param name="pagecount">满足指定条件数据可分页总数</param>
        /// <param name="hasnext">是否含有下一页</param>
        /// <param name="orderby">排序</param>
        /// <returns>返回满足条件的模型</returns>
        public virtual List<TMODEL> GetList (out int datacount, out int pagecount, out bool hasnext, TMODEL where, int pageIndex = 1, int pageSize = 20, params string[] orderby) {
            var db_where = ConvertWHERE (where, out TableInfo tbinfo);
            string Tsql = CreatePageTSQL (tbinfo, db_where, out DBParameters output, pageIndex, pageSize, orderby);
            logger?.LogDebug (Tsql);
            List<DBParameters> dbparamses = new List<DBParameters> ();
            dbparamses.AddRange (db_where.Values.ToList ());
            dbparamses.Add (output);
            dbparamses.Remove (null);
            var result = DataTableToList (base.server_connection.ExecuteSQLQuery (Tsql, dbparamses));

            datacount = Convert.ToInt32 (output.Value);
            pagecount = (int) Math.Ceiling (datacount / (double) pageSize);
            hasnext = pageIndex < pagecount;
            return result;
        }

        /// <summary>
        /// 将DataTable转换为Model
        /// </summary>
        /// <param name="reader">数据集合</param>
        /// <returns>模型</returns>
        protected virtual TMODEL DataTableToModel (DbDataReader reader) {
            List<TMODEL> list = DataTableToList (reader);
            return list != null ? list[0] : null;
        }

        /// <summary>
        /// 将DataTable转换为List
        /// </summary>
        /// <param name="reader">数据集合</param>
        /// <returns>模型集合</returns>
        protected virtual List<TMODEL> DataTableToList (DbDataReader reader) {
            DataTable dt = new DataTable (Guid.NewGuid ().ToString ("N"));
            dt.Load (reader);
            return DataTableToList (dt);
        }

        /// <summary>
        /// 将DataTable转换为Model
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <returns>模型</returns>
        protected virtual TMODEL DataTableToModel (DataTable dt) {
            List<TMODEL> list = DataTableToList (dt);
            return list != null ? list[0] : null;
        }

        /// <summary>
        /// 将DataTable转换为List
        /// </summary>
        /// <param name="dt">数据集合</param>
        /// <returns>模型集合</returns>
        protected virtual List<TMODEL> DataTableToList (DataTable dt) {
            List<TMODEL> result = null;
            PropertyInfo[] properties = typeof (TMODEL).GetProperties ();
            if (dt?.Rows?.Count > 0) {
                result = new List<TMODEL> ();
                foreach (DataRow dr in dt.Rows) {
                    TMODEL t = new TMODEL ();
                    foreach (PropertyInfo pi in properties) {
                        foreach (DataColumn col in dt.Columns) {
                            if (pi.Name.ToLower () == col.ColumnName.ToLower () && pi.CanWrite) {
                                var dr_value = dr[col];
                                if (dr_value == DBNull.Value) break;
                                Type vtype = pi.PropertyType;
                                vtype = Nullable.GetUnderlyingType (vtype) ?? vtype;
                                if (typeof (Enum).IsAssignableFrom (vtype)) {
                                    try {
                                        pi.SetValue (t, Enum.Parse (vtype, dr_value.ToString ()), null);
                                    } catch {
                                        pi.SetValue (t, null, null);
                                    }
                                } else {
                                    pi.SetValue (t, dr_value, null);
                                }
                            }
                        }
                    }
                    result.Add (t);
                }
            }
            return result;
        }

        /// <summary>
        /// 将模型转换为对应的表信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="tbinfo">表信息</param>
        /// <returns>参数集合key:表字段名，value:参数实例</returns>
        protected Dictionary<string, DBParameters> ConvertMODEL (TMODEL model, out TableInfo tbinfo) {
            return ConvertMODEL (model, out tbinfo, true, null);
        }

        /// <summary>
        /// 将模型转换为对应的表信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="tbinfo">表信息</param>
        /// <returns>参数集合key:表字段名，value:参数实例</returns>
        protected Dictionary<string, DBParameters> ConvertWHERE (TMODEL model, out TableInfo tbinfo) {
            return ConvertMODEL (model, out tbinfo, false, "w_");
        }
        /// <summary>
        /// 将模型转换为对应的表信息
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="tbinfo">表信息</param>
        /// <param name="prefix">生成参数前缀名</param>
        /// <param name="ignore_identity">忽略自增列</param>
        /// <returns>参数集合key:表字段名，value:参数实例</returns>
        Dictionary<string, DBParameters> ConvertMODEL (TMODEL model, out TableInfo tbinfo, bool ignore_identity, string prefix) {
            Dictionary<string, DBParameters> dbparameters = new Dictionary<string, DBParameters> ();
            tbinfo = new TableInfo ();
            var type = model.GetType ();
            if (!type.IsDefined (typeof (TableAttribute))) {
                throw new Exception ($"{nameof(model)}未定义table属性");
            }

            var attr_tabl = (TableAttribute) type.GetCustomAttribute (typeof (TableAttribute));
            tbinfo.TableName = attr_tabl.TableName;
            tbinfo.TableASName = attr_tabl.ASName;

            bool isfound_pkkey = false;
            PropertyInfo[] properties = type.GetProperties ();
            foreach (PropertyInfo pi in properties) {
                string name = pi.Name;
                object value = null;
                tbinfo.Fields.Add (name);
                if (pi.IsDefined (typeof (TBPKAttribute))) {
                    if (isfound_pkkey) {
                        throw new Exception ($"{nameof(model)}存在多个PkKey属性");
                    }
                    isfound_pkkey = true;
                    tbinfo.PkKey = name;
                }

                if (!pi.CanRead || (ignore_identity && pi.IsDefined (typeof (TBIdentityAttribute))) || (value = pi.GetValue (model, null)) == null) {
                    continue;
                }

                Type vtype = pi.PropertyType;
                vtype = Nullable.GetUnderlyingType (vtype) ?? vtype;

                if (pi.IsDefined (typeof (TBEnumAttribute))) {
                    var attr = (TBEnumAttribute) pi.GetCustomAttribute (typeof (TBEnumAttribute));
                    if (attr.CurrentAccess == TBEnumAttribute.Access.Name) {
                        value = System.Enum.Parse (vtype, value.ToString ()).ToString ();
                    } else {
                        value = (int) System.Enum.Parse (vtype, value.ToString ());
                    }
                }

                string p_name = $"{name}";
                if (dbparameters.ContainsKey (p_name)) {
                    throw new Exception ($"{nameof(model)}中含有重复参数{name}");
                }
                dbparameters.Add (p_name, new DBParameters () {
                    ParameterName = $"@{prefix}{p_name}",
                        Value = value
                });
            }

            if (!isfound_pkkey) {
                throw new Exception ($"{nameof(model)}未定义PkKey属性");
            }
            return dbparameters;
        }

        TableInfo _TBInfo;
        /// <summary>
        /// 表信息
        /// </summary>
        TableInfo TBInfo {
            get {
                if (_TBInfo == null) {
                    _TBInfo = new TableInfo ();
                    var type = typeof (TMODEL);
                    if (!type.IsDefined (typeof (TableAttribute))) {
                        throw new Exception ($"{nameof(TMODEL)}未定义table属性");
                    }

                    var attr_tabl = (TableAttribute) type.GetCustomAttribute (typeof (TableAttribute));
                    _TBInfo.TableName = attr_tabl.TableName;
                    _TBInfo.TableASName = attr_tabl.ASName;
                    PropertyInfo[] properties = type.GetProperties ();
                    foreach (PropertyInfo pi in properties) {
                        string name = pi.Name;
                        _TBInfo.Fields.Add (name);
                        if (pi.IsDefined (typeof (TBPKAttribute))) {
                            _TBInfo.PkKey = name;
                        }
                    }
                }
                return _TBInfo;
            }
        }

        /// <summary>
        /// 构建Select语句
        /// </summary>
        /// <param name="tbinfo">表信息</param>
        /// <param name="whereparameters">条件参数</param>
        /// <param name="orderby">排序</param>
        /// <returns>Select语句</returns>
        public abstract string CreateSelectTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters, params string[] orderby);

        /// <summary>
        /// 构建Select Count 语句
        /// </summary>
        /// <param name="tbinfo">表信息</param>
        /// <param name="whereparameters">条件参数</param>
        /// <returns>Select Count 语句</returns>
        public abstract string CreateSelectCountTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters);

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
        public abstract string CreatePageTSQL (TableInfo tbinfo, Dictionary<string, DBParameters> whereparameters, out Connection.DBParameters output, int pageIndex = 1, int pageSize = 20, params string[] orderby);

    }
}