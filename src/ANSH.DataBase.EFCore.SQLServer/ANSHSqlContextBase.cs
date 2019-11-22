using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// EF操作SqlServer
    /// </summary>
    public abstract class ANSHSqlContextBase<TEntity> : ANSHDbContextOptionsBase<TEntity> where TEntity : class, IANSHDbEntityBase, new () {

        bool UseRowNumberForPaging {
            get;
        }

        bool EnableRetryOnFailure {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="useRowNumberForPaging">是否使用RowNumber分页</param>
        /// <param name="enableRetryOnFailure">是否使用连接复原</param>
        public ANSHSqlContextBase (bool useRowNumberForPaging = false, bool enableRetryOnFailure = false) {
            this.UseRowNumberForPaging = useRowNumberForPaging;
            this.EnableRetryOnFailure = enableRetryOnFailure;
        }

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected override void OnConfiguring (DbContextOptionsBuilder dbOptionsBuilder) {
            dbOptionsBuilder.EnableSensitiveDataLogging ();
            if (!dbOptionsBuilder.IsConfigured) {
                dbOptionsBuilder
                    .UseSqlServer (ANSHDbConnection.DbConnection, m => {
                        if (UseRowNumberForPaging) {
                            m.UseRowNumberForPaging ();
                        }
                        if (EnableRetryOnFailure) {
                            m.EnableRetryOnFailure ();
                        }
                    });
            }
            //dbOptionsBuilder.ConfigureWarnings (warnings => warnings.Throw (RelationalEventId.QueryClientEvaluationWarning));
            if (Loggers != null) {
                dbOptionsBuilder.UseLoggerFactory (Loggers);
            }
        }
    }
}