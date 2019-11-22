using System;
using System.Collections.Generic;
using System.Data.Common;
using ANSH.DataBase.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// EF操作基类
    /// </summary>
    public abstract class ANSHDbContextBase : DbContext {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHDbContextBase () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="anshDbConnection">数据库连接对象</param>
        /// <param name="loggers">日志记录</param>
        public ANSHDbContextBase (ANSHDbConnection anshDbConnection, ILoggerFactory loggers) {
            UseConnection (anshDbConnection, loggers);
        }

        /// <summary>
        /// 数据库连接
        /// </summary>
        ANSHDbConnection _ANSHDbConnection {
            get;
            set;
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public ANSHDbConnection ANSHDbConnection => _ANSHDbConnection;

        /// <summary>
        /// 日志记录
        /// </summary>
        ILoggerFactory _Loggers {
            get;
            set;
        }
        /// <summary>
        /// 日志记录
        /// </summary>
        public ILoggerFactory Loggers => _Loggers;

        /// <summary>
        /// 使用数据库连接
        /// </summary>
        /// <param name="anshDbConnection">数据库连接对象</param>
        /// <param name="loggers">日志记录</param>
        public void UseConnection (ANSHDbConnection anshDbConnection, ILoggerFactory loggers = null) {
            _ANSHDbConnection = anshDbConnection;
            _Loggers = loggers;
            UserTransaction ();
        }

        /// <summary>
        /// 共享事物保护
        /// </summary>
        public void UserTransaction () {
            UserTransaction (ANSHDbConnection.DbTransaction);
        }

        /// <summary>
        /// 共享事物保护
        /// </summary>
        public void UserTransaction (DbTransaction dbTransaction) {
            if (dbTransaction != null && base.Database.CurrentTransaction == null) {
                base.Database.UseTransaction (dbTransaction);
            }
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (ANSHDbContextBase db) {
            _DBContext.Add (db);
        }

        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<ANSHDbContextBase> _DBContext {
            get;
            set;
        } = new List<ANSHDbContextBase> ();

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="method">事物保护的方法</param>
        protected void ExecuteTransaction (Action method) {
            try {
                ANSHDbConnection.BeginTransaction ();
                UserTransaction ();
                method ();
                ANSHDbConnection.Commit ();
            } catch (Exception ex) {
                ANSHDbConnection.Rollback ();
                throw ex;
            }
        }

        /// <summary>
        /// 创建对应的BLL层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        [Obsolete ("替换为Register<TResult>()", true)]
        new public TResult Set<TResult> ()
        where TResult : ANSHDbContextBase, new () {
            var result = new TResult ();
            result.UseConnection (_ANSHDbConnection);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 创建对应的BLL层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public TResult Register<TResult> ()
        where TResult : ANSHDbContextBase, new () {
            var result = new TResult ();
            result.UseConnection (_ANSHDbConnection);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 创建DbSet实体
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <returns>返回对应的DbSet实体</returns>
        public DbSet<TEntity> RegisterEntity<TEntity> () where TEntity : class => base.Set<TEntity> ();

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose () {
            base.Dispose ();
            _DBContext?.ForEach (m => m.Dispose ());
        }
    }
}