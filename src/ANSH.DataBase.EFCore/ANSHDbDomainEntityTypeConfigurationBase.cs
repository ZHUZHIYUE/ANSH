using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANSH.DataBase.EFCore {

    /// <summary>
    /// 实体类型的配置基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPKey">实体主键类型</typeparam>
    public abstract class ANSHDbDomainEntityTypeConfigurationBase<TEntity, TPKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IANSHDbDomainEntityBase<TPKey>
        where TPKey : struct, IEquatable<TPKey> {

            /// <summary>
            /// 配置类型TEntity的实体
            /// </summary>
            /// <param name="builder">用于配置实体类型的生成器</param>
            public void Configure (EntityTypeBuilder<TEntity> builder) {
                builder.HasKey (m => m.Id);
                builder.Property (m => m.Id).ValueGeneratedOnAdd ();
                builder.Property (m => m.UpdateTimes).HasDefaultValue(DateTime.Now);
                builder.Property (m => m.CreateTimes).HasDefaultValue(DateTime.Now);
                builder.Property (m => m.Timestamp).IsRowVersion ();
                ConfigureBuilder (builder);
            }

            /// <summary>
            /// 配置类型TEntity的实体
            /// </summary>
            /// <param name="builder">用于配置实体类型的生成器</param>
            public abstract void ConfigureBuilder (EntityTypeBuilder<TEntity> builder);
        }
}