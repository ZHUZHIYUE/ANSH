using System;
using System.Collections.Generic;
using System.Data;
using ANSH.DataBase.Connection;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// 数据库操作入口
    /// </summary>
    public abstract class DBOptions : IDisposable {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="db_connection">数据库连接</param>
        public DBOptions (DBConnection db_connection) {
            _db_connection = db_connection;
        }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        string _connection {
            get;
            set;
        }

        /// <summary>
        /// 数据库链接及事物
        /// </summary>
        DBConnection _db_connection {
            get;
            set;
        } = null;

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<DBContext> _dbContext {
            get;
            set;
        } = new List<DBContext> ();

        /// <summary>
        /// 创建对应的访问层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Set<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult ();
            result.UseConnection (_db_connection);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (DBContext db) {
            _dbContext.Add (db);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose () {
            _db_connection?.Dispose ();
            _dbContext?.ForEach (m => m.Dispose ());
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        public void ExecuteTransaction (Action Method) {
            try {
                _db_connection.BeginTransaction ();
                Method ();
                _db_connection.Commit ();
            } catch (Exception ex) {
                _db_connection.Rollback ();
                throw ex;
            }
        }
    }
}