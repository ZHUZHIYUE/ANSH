using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// EF操作SqlServer
    /// </summary>
    public abstract class SQLContext<TEntity> : DBContextOptions<TEntity> where TEntity : DBEntity, new () {
        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguringsOptions (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder
                .UseLoggerFactory (Loggers)
                .UseSqlServer (DB_Connection.Connection, m => m.UseRowNumberForPaging ())
                .ConfigureWarnings (warnings => warnings.Throw (RelationalEventId.QueryClientEvaluationWarning));
        }
    }
}