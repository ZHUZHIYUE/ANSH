using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// EF操作SqlServer
    /// </summary>
    public abstract class ANSHSqlContextTypeConfigurationBase<TEntity, TIEntityTypeConfiguration> : ANSHSqlContextBase<TEntity>
        where TIEntityTypeConfiguration : IEntityTypeConfiguration<TEntity>, new ()
    where TEntity : class, IANSHDbEntityBase, new () {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHSqlContextTypeConfigurationBase () : base (false) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enableRetryOnFailure">是否使用连接复原</param>
        public ANSHSqlContextTypeConfigurationBase (bool enableRetryOnFailure) : base (enableRetryOnFailure) { }

        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="modelBuilder">模型绑定</param>
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration (new TIEntityTypeConfiguration ());
        }
    }
}