using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace ANSH.DataBase.Connection {

    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DBConnection : IDisposable {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接</param>
        public DBConnection (DbConnection connection) {
            _Connection = connection;
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public void Open () {
            _Connection.Open ();
        }

        /// <summary>
        /// SQL链接
        /// </summary>
        DbConnection _Connection;
        /// <summary>
        /// SQL链接
        /// </summary>
        public DbConnection Connection => _Connection;

        /// <summary>
        /// 数据库事务
        /// </summary>
        DbTransaction _Transaction;
        /// <summary>
        /// 数据库事务
        /// </summary>
        public DbTransaction Transaction => _Transaction;

        /// <summary>
        /// 开启事物次数
        /// </summary>
        int TranCount = 0;

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        /// <param name="isolationLevel">隔离级别</param>
        public void BeginTransaction (IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
            if (TranCount == 0) {
                if (_Connection?.State != ConnectionState.Open) {
                    Open ();
                }
                _Transaction = _Connection.BeginTransaction (isolationLevel);
            }
            TranCount++;
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        public void Commit () {
            TranCount--;

            if (TranCount == 0) {
                _Transaction?.Commit ();
                _Transaction?.Dispose ();
                _Transaction = null;
            }
        }

        /// <summary>
        /// 回滚数据库事务
        /// </summary>
        public void Rollback () {
            if (TranCount > 0) {
                _Transaction?.Rollback ();
                _Transaction?.Dispose ();
                _Transaction = null;
            }
            TranCount = 0;
        }

        /// <summary>
        /// 释放数据库连接资源
        /// </summary>
        public void Dispose () {
            _Transaction?.Dispose ();
            _Transaction = null;
            _Connection.Dispose ();
        }

        /// <summary>
        /// 执行查询，并返回结果集中第一行的第一列
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="DBParameters">SQL参数</param>
        /// <param name="com_type">Command类型</param>
        /// <returns>第一行的第一列</returns>
        public object ExecuteSQLScalar (string SQLString, List<DBParameters> DBParameters = null, CommandType com_type = CommandType.Text) {
            lock (_Connection) {
                using (DbCommand cmd = CreateCmd (SQLString, DBParameters, com_type, _Connection, _Transaction, out List<DbParameter> OutDBPatamters)) {
                    var result = cmd.ExecuteScalar ();
                    SETDBOutParameters (DBParameters, OutDBPatamters);
                    return result;
                }
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="DBParameters">SQL参数</param>
        /// <param name="com_type">Command类型</param>
        /// <returns>返回查询得到的数据</returns>
        public DataTable ExecuteSQLQuery (string SQLString, List<DBParameters> DBParameters = null, CommandType com_type = CommandType.Text) {
            lock (_Connection) {
                using (DbCommand cmd = CreateCmd (SQLString, DBParameters, com_type, _Connection, _Transaction, out List<DbParameter> OutDBPatamters)) {
                    DataTable dt = new DataTable (Guid.NewGuid ().ToString ("N"));
                    using (var result = cmd.ExecuteReader ()) {
                        dt.Load (result);
                    }
                    SETDBOutParameters (DBParameters, OutDBPatamters);
                    return dt;
                }
            }
        }

        ///<summary>
        /// 执行查询，并返回受影响的行数。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="DBParameters">SQL参数</param>
        /// <param name="com_type">Command类型</param>
        /// <returns>影响的行数</returns>
        public int ExecuteSQLNonQuery (string SQLString, List<DBParameters> DBParameters = null, CommandType com_type = CommandType.Text) {
            lock (_Connection) {
                using (DbCommand cmd = CreateCmd (SQLString, DBParameters, com_type, _Connection, _Transaction, out List<DbParameter> OutDBPatamters)) {
                    var result = cmd.ExecuteNonQuery ();
                    SETDBOutParameters (DBParameters, OutDBPatamters);
                    return result;
                }
            }
        }

        /// <summary>
        /// 构造SqlCommand
        /// </summary>
        /// <param name="CommandText">设置要对数据源执行的SQL语句、表名或存储过程。</param>
        /// <param name="DBParameters">SQL参数</param>
        /// <param name="cmdType">指定如何解释命令字符串</param>
        /// <param name="Connection">数据库打开链接</param>
        /// <param name="Transaction">数据库事物</param>
        /// <param name="result">参数</param>
        /// <returns>返回SqlCommand</returns>
        DbCommand CreateCmd (string CommandText, List<DBParameters> DBParameters, CommandType cmdType, DbConnection Connection, DbTransaction Transaction, out List<DbParameter> result) {
            if (Connection?.State != ConnectionState.Open) {
                Open ();
            }
            DbCommand cmd = Connection.CreateCommand ();
            cmd.CommandText = CommandText;
            cmd.CommandType = cmdType;
            cmd.Transaction = Transaction;
            result = new List<DbParameter> ();

            foreach (var in_DBOutParameters in DBParameters ?? new List<DBParameters> ()) {
                var dbparameter = cmd.CreateParameter ();
                dbparameter.ParameterName = in_DBOutParameters.ParameterName.TrimStart ('@');
                dbparameter.Value = in_DBOutParameters.Value;
                dbparameter.Size = in_DBOutParameters.Size ?? 0;
                dbparameter.Direction = in_DBOutParameters.Direction;
                if (in_DBOutParameters.DbType.HasValue) {
                    dbparameter.DbType = in_DBOutParameters.DbType.Value;
                }
                cmd.Parameters.Add (dbparameter);
                result.Add (dbparameter);
            }
            result = result?.Count > 0 ? result : null;
            return cmd;
        }

        /// <summary>
        /// 为输出参数赋值
        /// </summary>
        /// <param name="DBParameters">传入的输出参数</param>
        /// <param name="result">生成的输出参数</param>
        void SETDBOutParameters (List<DBParameters> DBParameters, List<DbParameter> result) {
            var left = DBParameters?.FindAll (m => m.Direction == ParameterDirection.Output);
            var right = result?.FindAll (m => m.Direction == ParameterDirection.Output);
            if (left?.Count > 0 && right?.Count > 0) {
                left.ForEach (m => m.Value = right?.Find (k => k.ParameterName.TrimStart ('@') == m.ParameterName.TrimStart ('@'))?.Value);
            }
        }
    }
}