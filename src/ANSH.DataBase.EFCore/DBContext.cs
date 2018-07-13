using System;
using System.Collections.Generic;
using ANSH.DataBase.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// EF操作基类
    /// </summary>
    public abstract class DBContext : DbContext {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBContext () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbconnection">数据库连接对象</param>
        public DBContext (DBConnection dbconnection) {
            UseConnection (dbconnection);
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        DBConnection _DB_Connection {
            get;
            set;
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public DBConnection DB_Connection {
            get => _DB_Connection;
        }

        /// <summary>
        /// 使用数据库连接
        /// </summary>
        /// <param name="dbconnection">数据库连接对象</param>
        public void UseConnection (DBConnection dbconnection) {
            _DB_Connection = dbconnection;
            UserTransaction ();
        }

        /// <summary>
        /// 共享事物保护
        /// </summary>
        void UserTransaction () {
            if (_DB_Connection.Transaction != null && base.Database.CurrentTransaction == null) {
                base.Database.UseTransaction (_DB_Connection.Transaction);
            }
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (DBContext db) {
            _dbContext.Add (db);
        }

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<DBContext> _dbContext {
            get;
            set;
        } = new List<DBContext> ();

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        protected void ExecuteTransaction (Action Method) {
            try {
                _DB_Connection.BeginTransaction ();
                UserTransaction ();
                Method ();
                _DB_Connection.Commit ();
            } catch (Exception ex) {
                _DB_Connection.Rollback ();
                throw ex;
            }
        }

        /// <summary>
        /// 创建对应的BLL层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        new public TResult Set<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult ();
            result.UseConnection (_DB_Connection);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            OnConfiguringsOptions (optionsBuilder);
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected abstract void OnConfiguringsOptions (DbContextOptionsBuilder optionsBuilder);

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose () {
            base.Dispose ();
            _dbContext?.ForEach (m => m.Dispose ());
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        protected static readonly LoggerFactory Loggers
            = new LoggerFactory (new [] { new ConsoleLoggerProvider ((_, __) => true, true) });
    }
}