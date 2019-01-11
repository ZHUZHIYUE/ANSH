using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DataBase.EFCore.SQLServer {
    /// <summary>
    /// EF操作SqlServer
    /// </summary>
    public abstract class SQLContextTypeConfiguration<TEntity, TIEntityTypeConfiguration> : SQLContext<TEntity>
        where TIEntityTypeConfiguration : IEntityTypeConfiguration<TEntity>, new ()
    where TEntity : class, IDBEntity, new () {

        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="modelBuilder">模型绑定</param>
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration (new TIEntityTypeConfiguration ());
        }
    }
}