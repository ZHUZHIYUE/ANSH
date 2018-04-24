using System;
using ANSH.DataBase.Connection;
using Microsoft.EntityFrameworkCore;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// EF操作基类
    /// </summary>
    public abstract class DBContext : DbContext {
        /// <summary>
        /// 数据库链接
        /// </summary>
        public DBConnection DB_Connection {
            get;
            set;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        public Action<DBContext> AddDbContext {
            get;
            set;
        }

        /// <summary>
        /// 事物保护
        /// </summary>
        /// <param name="Method">事物保护的方法</param>
        public void ExecuteTransaction (Action Method) {
            try {
                UserTransaction ();
                DB_Connection.BeginTransaction ();
                UserTransaction ();
                Method ();
                DB_Connection.Commit ();
            } catch (Exception ex) {
                DB_Connection.Rollback ();
                throw ex;
            }
        }

        /// <summary>
        /// 共享事物保护
        /// </summary>
        public void UserTransaction () {
            if (DB_Connection.Transaction != null && base.Database.CurrentTransaction == null) {
                base.Database.UseTransaction (DB_Connection.Transaction);
            }
        }

        /// <summary>
        /// 创建对应的BLL层对象
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        new public TResult Set<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult () { DB_Connection = DB_Connection };
            result.UserTransaction ();
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            OnConfiguringsOptions(optionsBuilder);
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected abstract void OnConfiguringsOptions (DbContextOptionsBuilder optionsBuilder);
    }
}