using System;
using System.Collections.Generic;
using System.Threading;
using ANSH.DataBase.Connection;
using ANSH.DataBase.EFCore;
using ANSH.DataBase.IUnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.IUnitOfWorks.EFCore {

    /// <summary>
    /// EFCore仓储操作工作基类
    /// </summary>
    public class ANSHBaseEFCoreUnitOfWork : ANSHBaseUnitOfWork, IANSHEFCoreUnitOfWork {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="db_connection">数据库连接</param>
        /// <param name="loggerfactory">日志记录</param>
        public ANSHBaseEFCoreUnitOfWork (DBConnection db_connection, ILoggerFactory loggerfactory = null) : base (db_connection, loggerfactory) { }

      
        /// <summary>
        /// 创建DbContext集合
        /// </summary>
        List<DbContext> _DbContext = new List<DbContext> ();

        /// <summary>
        /// 创建对应的访问层对象
        /// <remarks>创建的对象都一直保存在集合中，直到集合批量Dispose。</remarks>
        /// </summary>
        /// <typeparam name="TResult">对应的BLL层对象</typeparam>
        /// <returns>返回对应的BLL层对象</returns>
        public virtual TResult Register<TResult> ()
        where TResult : DBContext, new () {
            var result = new TResult ();
            result.UseConnection (base.DBconnection, base.Loggerfactory);
            AddDbContext (result);
            return result;
        }

        /// <summary>
        /// 添加DbContext记录
        /// </summary>
        /// <param name="db"></param>
        void AddDbContext (DBContext db) {
            _DbContext.Add (db);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose () {
            base.Dispose ();
            _DbContext?.ForEach (m => m.Dispose ());
            _DbContext?.Clear ();
        }
    }
}