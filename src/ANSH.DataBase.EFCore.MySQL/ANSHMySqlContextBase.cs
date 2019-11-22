using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace ANSH.DataBase.EFCore.MySQL {
    /// <summary>
    /// EF操作MySQL
    /// </summary>
    public abstract class ANSHMySqlContextBase<TEntity> : ANSHDbContextOptionsBase<TEntity> where TEntity : ANSHDbEntityBase, new () {
        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder dbOptionsBuilder) {
            dbOptionsBuilder.EnableSensitiveDataLogging ();
            dbOptionsBuilder
                .UseMySQL (ANSHDbConnection.DbConnection);
                //.ConfigureWarnings (warnings => warnings.Throw (RelationalEventId.QueryClientEvaluationWarning));

            if (Loggers != null) {
                dbOptionsBuilder.UseLoggerFactory (Loggers);
            }
        }
    }
}