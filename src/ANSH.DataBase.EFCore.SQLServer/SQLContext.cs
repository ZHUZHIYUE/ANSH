using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// EF操作SqlServer
    /// </summary>
    public abstract class SQLContext<TEntity> : DBContextOptions<TEntity> where TEntity : class, IDBEntity, new () {

        bool UseRowNumberForPaging {
            get;
        }

        bool EnableRetryOnFailure {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UseRowNumberForPaging">是否使用RowNumber分页</param>
        /// <param name="EnableRetryOnFailure">是否使用连接复原</param>
        public SQLContext (bool UseRowNumberForPaging = false, bool EnableRetryOnFailure = false) {
            this.UseRowNumberForPaging = UseRowNumberForPaging;
            this.EnableRetryOnFailure = EnableRetryOnFailure;
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder OptionsBuilder) {
            if (!OptionsBuilder.IsConfigured) {
                OptionsBuilder
                    .UseSqlServer (DB_Connection.Connection, m => {
                        if (UseRowNumberForPaging) {
                            m.UseRowNumberForPaging ();
                        }
                        if (EnableRetryOnFailure) {
                            m.EnableRetryOnFailure ();
                        }
                    });
            }
            OptionsBuilder.ConfigureWarnings (warnings => warnings.Throw (RelationalEventId.QueryClientEvaluationWarning));
            if (Loggers != null) {
                OptionsBuilder.UseLoggerFactory (Loggers);
            }
        }
    }
}