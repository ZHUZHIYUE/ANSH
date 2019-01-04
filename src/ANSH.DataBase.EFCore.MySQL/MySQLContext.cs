using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace ANSH.DataBase.EFCore.MySQL {
    /// <summary>
    /// EF操作MySQL
    /// </summary>
    public abstract class MySQLContext<TEntity> : DBContextOptions<TEntity> where TEntity : DBEntity, new () {
        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder OptionsBuilder) {
            OptionsBuilder
                .UseMySQL (DB_Connection.Connection)
                .ConfigureWarnings (warnings => warnings.Throw (RelationalEventId.QueryClientEvaluationWarning));

            if (Loggers != null) {
                OptionsBuilder.UseLoggerFactory (Loggers);
            }
        }

        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="modelBuilder">模型绑定</param>
        new protected abstract void OnModelCreating (ModelBuilder modelBuilder);
    }
}