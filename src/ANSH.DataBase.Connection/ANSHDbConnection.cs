using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace ANSH.DataBase.Connection {

    /// <summary>
    /// 数据库连接
    /// </summary>
    public class ANSHDbConnection : IDisposable {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接</param>
        public ANSHDbConnection (DbConnection dbConnection) {
            _DbConnection = dbConnection;
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public void Open () {
            _DbConnection.Open ();
        }

        /// <summary>
        /// SQL链接
        /// </summary>
        DbConnection _DbConnection;
        /// <summary>
        /// SQL链接
        /// </summary>
        public DbConnection DbConnection => _DbConnection;

        /// <summary>
        /// 数据库事务
        /// </summary>
        DbTransaction _DbTransaction;
        /// <summary>
        /// 数据库事务
        /// </summary>
        public DbTransaction DbTransaction => _DbTransaction;

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
                if (_DbConnection?.State != ConnectionState.Open) {
                    Open ();
                }
                _DbTransaction = _DbConnection.BeginTransaction (isolationLevel);
            }
            TranCount++;
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        public void Commit () {
            TranCount--;

            if (TranCount == 0) {
                _DbTransaction?.Commit ();
                _DbTransaction?.Dispose ();
                _DbTransaction = null;
            }
        }

        /// <summary>
        /// 回滚数据库事务
        /// </summary>
        public void Rollback () {
            if (TranCount > 0) {
                _DbTransaction?.Rollback ();
                _DbTransaction?.Dispose ();
                _DbTransaction = null;
            }
            TranCount = 0;
        }

        /// <summary>
        /// 释放数据库连接资源
        /// </summary>
        public void Dispose () {
            _DbTransaction?.Dispose ();
            _DbTransaction = null;
            _DbConnection.Dispose ();
        }

        /// <summary>
        /// 执行查询，并返回结果集中第一行的第一列
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="dbParameters">SQL参数</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>第一行的第一列</returns>
        public object ExecuteSQLScalar (string sqlString, List<ANSHDbParameter> dbParameters = null, CommandType commandType = CommandType.Text) {

            using (DbCommand cmd = CreateCmd (sqlString, dbParameters, commandType, out List<DbParameter> OutDBPatamters)) {
                var result = cmd.ExecuteScalar ();
                SETDBOutParameters (dbParameters, OutDBPatamters);
                return result;
            }

        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="anshDbParameters">SQL参数</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>返回查询得到的数据</returns>
        public DataTable ExecuteSQLQuery (string sqlString, List<ANSHDbParameter> anshDbParameters = null, CommandType commandType = CommandType.Text) {
            using (var result = ExecuteSQLQueryAsDbDataReader (sqlString, anshDbParameters, commandType)) {
                using (DbCommand cmd = CreateCmd (sqlString, anshDbParameters, commandType, out List<DbParameter> OutDBPatamters)) {
                    DataTable dt = new DataTable (Guid.NewGuid ().ToString ("N"));
                    dt.Load (result);
                    SETDBOutParameters (anshDbParameters, OutDBPatamters);
                    return dt;
                }
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="anshDbParameters">SQL参数</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>返回查询得到的数据</returns>
        public DbDataReader ExecuteSQLQueryAsDbDataReader (string sqlString, List<ANSHDbParameter> anshDbParameters = null, CommandType commandType = CommandType.Text) {
            using (DbCommand cmd = CreateCmd (sqlString, anshDbParameters, commandType, out List<DbParameter> OutDBPatamters)) {
                return cmd.ExecuteReader ();
            }
        }

        ///<summary>
        /// 执行查询，并返回受影响的行数。
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="anshDbParameters">SQL参数</param>
        /// <param name="commandType">Command类型</param>
        /// <returns>影响的行数</returns>
        public int ExecuteSQLNonQuery (string sqlString, List<ANSHDbParameter> anshDbParameters = null, CommandType commandType = CommandType.Text) {
            using (DbCommand cmd = CreateCmd (sqlString, anshDbParameters, commandType, out List<DbParameter> OutDBPatamters)) {
                var result = cmd.ExecuteNonQuery ();
                SETDBOutParameters (anshDbParameters, OutDBPatamters);
                return result;
            }
        }

        /// <summary>
        /// 构造SqlCommand
        /// </summary>
        /// <param name="commandText">设置要对数据源执行的SQL语句、表名或存储过程。</param>
        /// <param name="anshDbParameters">SQL参数</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="resultDbParameter">参数</param>
        /// <returns>返回SqlCommand</returns>
        DbCommand CreateCmd (string commandText, List<ANSHDbParameter> anshDbParameters, CommandType commandType, out List<DbParameter> resultDbParameter) {
            if (DbConnection?.State != ConnectionState.Open) {
                Open ();
            }
            DbCommand cmd = DbConnection.CreateCommand ();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            cmd.Transaction = DbTransaction;
            resultDbParameter = CreateDbParameter (cmd, anshDbParameters);
            if (resultDbParameter?.Count > 0) {
                cmd.Parameters.AddRange (resultDbParameter.ToArray ());
            }
            return cmd;
        }

        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="dbCommand">SqlCommand</param>
        /// <param name="anshDbParameters">待转换参数</param>
        /// <returns>转换后的参数</returns>
        public List<DbParameter> CreateDbParameter (DbCommand dbCommand, List<ANSHDbParameter> anshDbParameters) {
            List<DbParameter> result = new List<DbParameter> ();
            foreach (var in_DBOutParameters in anshDbParameters ?? new List<ANSHDbParameter> ()) {
                var dbparameter = dbCommand.CreateParameter ();
                dbparameter.ParameterName = in_DBOutParameters.ParameterName.TrimStart ('@');
                dbparameter.Value = in_DBOutParameters.Value;
                dbparameter.Size = in_DBOutParameters.Size ?? 0;
                dbparameter.Direction = in_DBOutParameters.Direction;
                if (in_DBOutParameters.DbType.HasValue) {
                    dbparameter.DbType = in_DBOutParameters.DbType.Value;
                }
                result.Add (dbparameter);
            }
            return result = result?.Count > 0 ? result : null;
        }

        /// <summary>
        /// 为输出参数赋值
        /// </summary>
        /// <param name="anshDbParameters">传入的输出参数</param>
        /// <param name="resultDbParameter">生成的输出参数</param>
        void SETDBOutParameters (List<ANSHDbParameter> anshDbParameters, List<DbParameter> resultDbParameter) {
            var left = anshDbParameters?.FindAll (m => m.Direction == ParameterDirection.Output);
            var right = resultDbParameter?.FindAll (m => m.Direction == ParameterDirection.Output);
            if (left?.Count > 0 && right?.Count > 0) {
                left.ForEach (m => m.Value = right?.Find (k => k.ParameterName.TrimStart ('@') == m.ParameterName.TrimStart ('@'))?.Value);
            }
        }
    }
}