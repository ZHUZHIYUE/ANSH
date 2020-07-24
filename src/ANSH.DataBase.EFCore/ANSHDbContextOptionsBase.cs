using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ANSH.DataBase.Connection;
using ANSH.DDD.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// 操作实体基类
    /// </summary>
    /// <typeparam name="TEntity">实体模型</typeparam>
    public abstract class ANSHDbContextOptionsBase<TEntity> : ANSHDbContextBase where TEntity : class, IANSHDbEntityBase, new () {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ANSHDbContextOptionsBase () : base () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="anshDbConnection">数据库连接对象</param>
        /// <param name="loggers">日志记录</param>
        public ANSHDbContextOptionsBase (ANSHDbConnection anshDbConnection, ILoggerFactory loggers) : base (anshDbConnection, loggers) { }

        /// <summary>
        /// 实体对象
        /// </summary>
        public DbSet<TEntity> DbEntity { get; set; }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TEntity[] Insert (TEntity[] entity) {
            if ((entity ?? new TEntity[0]).Length > 0) {
                DbEntity.AddRange (entity);
                base.SaveChanges ();
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 批量添加实体
        /// /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>添加后的实体</returns>
        public virtual async Task<TEntity[]> InsertAsync (TEntity[] entity, CancellationToken cancellationToken = default) {
            if ((entity ?? new TEntity[0]).Length > 0) {
                DbEntity.AddRange (entity);
                await base.SaveChangesAsync (cancellationToken);
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TEntity Insert (TEntity entity) => InsertAsync (entity).Result;

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>添加后的实体</returns>
        public virtual async Task<TEntity> InsertAsync (TEntity entity, CancellationToken cancellationToken = default) {
            DbEntity.Add (entity);
            await base.SaveChangesAsync (cancellationToken);
            return entity;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TEntity Insert (Action<TEntity> entity) => InsertAsync (entity).Result;

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>添加后的实体</returns>
        public virtual async Task<TEntity> InsertAsync (Action<TEntity> entity, CancellationToken cancellationToken = default) {
            var insert = new TEntity ();
            entity?.Invoke (insert);
            if (insert == null) {
                return null;
            }
            DbEntity.Add (insert);
            await base.SaveChangesAsync (cancellationToken);
            return insert;
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="wheres">指定条件</param>
        public virtual void Update (Action<TEntity> entity, Expression<Func<TEntity, bool>> wheres) {
            Get (wheres, tracking : true).ToList ()?.ForEach ((item) => {
                entity (item);
                DbEntity.Update (item);
            });

            SaveChangesCoverage ();
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="wheres">指定条件</param>
        public virtual async Task UpdateAsync (Action<TEntity> entity, Expression<Func<TEntity, bool>> wheres, CancellationToken cancellationToken = default) {
            var result = await Get (wheres, tracking : true).ToListAsync (cancellationToken);
            result?.ForEach ((item) => {
                entity (item);
                DbEntity.Update (item);
            });

            await SaveChangesCoverageAsync (cancellationToken);
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="specification">规约</param>
        public virtual void Update (Action<TEntity> entity, IANSHSpecificationCommit<TEntity> specification) {
            Get (specification, tracking : true).ToList ().ForEach ((item) => {
                entity (item);
                DbEntity.Update (item);
            });

            if (specification?.CommitSaveChanges == CommitSaveChangesTypes.Reload) {
                SaveChangesReload ();
            } else {
                SaveChangesCoverage ();
            }
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="specification">规约</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task UpdateAsync (Action<TEntity> entity, IANSHSpecificationCommit<TEntity> specification, CancellationToken cancellationToken = default) {
            var result = await Get (specification, tracking : true).ToListAsync (cancellationToken);
            result?.ForEach ((item) => {
                entity (item);
                DbEntity.Update (item);
            });

            if (specification?.CommitSaveChanges == CommitSaveChangesTypes.Reload) {
                await SaveChangesReloadAsync (cancellationToken);
            } else {
                await SaveChangesCoverageAsync (cancellationToken);
            }
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="wheres">指定条件</param>
        /// <param name="cascade">联级删除</param>
        public virtual void Delete (Expression<Func<TEntity, bool>> wheres, Func<IQueryable<TEntity>, IQueryable<TEntity>> cascade = null) {
            var iqueryable = Get (wheres, tracking : true);
            if (cascade != null) iqueryable = cascade (iqueryable);
            DbEntity.RemoveRange (iqueryable);
            SaveChangesCoverage ();
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="wheres">指定条件</param>
        /// <param name="cascade">联级删除</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task DeleteAsync (Expression<Func<TEntity, bool>> wheres, Func<IQueryable<TEntity>, IQueryable<TEntity>> cascade = null, CancellationToken cancellationToken = default) {
            var iqueryable = Get (wheres, tracking : true);
            if (cascade != null) iqueryable = cascade (iqueryable);
            DbEntity.RemoveRange (iqueryable);
            await SaveChangesCoverageAsync (cancellationToken);
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="cascade">联级删除</param>
        public virtual void Delete (IANSHSpecificationCommit<TEntity> specification = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> cascade = null) {
            var iqueryable = Get (specification, tracking : true);
            if (cascade != null) iqueryable = cascade (iqueryable);
            DbEntity.RemoveRange (iqueryable);

            if (specification?.CommitSaveChanges == CommitSaveChangesTypes.Reload) {
                SaveChangesReload ();
            } else {
                SaveChangesCoverage ();
            }
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="cascade">联级删除</param>
        /// <param name="cancellationToken">取消令牌</param>
        public virtual async Task DeleteAsync (IANSHSpecificationCommit<TEntity> specification = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> cascade = null, CancellationToken cancellationToken = default) {
            var iqueryable = Get (specification, tracking : true);
            if (cascade != null) iqueryable = cascade (iqueryable);
            DbEntity.RemoveRange (iqueryable);

            if (specification?.CommitSaveChanges == CommitSaveChangesTypes.Reload) {
                await SaveChangesReloadAsync (cancellationToken);
            } else {
                await SaveChangesCoverageAsync (cancellationToken);
            }
        }

        /// <summary>
        /// 保存修改
        /// <para>解决乐观并发冲突，用提交数据覆盖数据库数据</para>
        /// </summary>
        void SaveChangesCoverage () {
            int repeat = 0;
            while (true) {
                try {
                    base.SaveChanges ();
                    break;
                } catch (DbUpdateConcurrencyException ex) {
                    foreach (var entry in ex.Entries) {
                        if (entry.Entity is TEntity) {
                            var databaseVaules = entry.GetDatabaseValues ();
                            if (databaseVaules != null) {
                                entry.OriginalValues.SetValues (databaseVaules);
                            }
                        } else {
                            throw new NotSupportedException (
                                "Don't know how to handle concurrency conflicts for " +
                                entry.Metadata.Name);
                        }
                    }
                    if (repeat++ >= 10) {
                        throw new ApplicationException ("尝试处理并发冲突次数超过10次。");
                    }
                }
            }
        }

        /// <summary>
        /// 保存修改
        /// <para>解决乐观并发冲突，用提交数据覆盖数据库数据</para>
        /// <param name="cancellationToken">取消令牌</param>
        /// </summary>
        async Task SaveChangesCoverageAsync (CancellationToken cancellationToken = default) {
            int repeat = 0;
            while (true) {
                try {
                    await base.SaveChangesAsync (cancellationToken);
                    break;
                } catch (DbUpdateConcurrencyException ex) {
                    foreach (var entry in ex.Entries) {
                        if (entry.Entity is TEntity) {
                            var databaseVaules = entry.GetDatabaseValues ();
                            if (databaseVaules != null) {
                                entry.OriginalValues.SetValues (databaseVaules);
                            }
                        } else {
                            throw new NotSupportedException (
                                "Don't know how to handle concurrency conflicts for " +
                                entry.Metadata.Name);
                        }
                    }
                    if (repeat++ >= 10) {
                        throw new ApplicationException ("尝试处理并发冲突次数超过10次。");
                    }
                }
            }
        }

        /// <summary>
        /// 保存修改
        /// <para>解决乐观并发冲突，重新加载数据库中的数据覆盖提交数据</para>
        /// </summary>
        void SaveChangesReload () {
            int repeat = 0;
            while (true) {
                try {
                    base.SaveChanges ();
                    break;
                } catch (DbUpdateConcurrencyException ex) {
                    foreach (var entry in ex.Entries) {
                        if (entry.Entity is TEntity) {
                            ex.Entries.Single ().Reload ();
                        } else {
                            throw new NotSupportedException (
                                "Don't know how to handle concurrency conflicts for " +
                                entry.Metadata.Name);
                        }
                    }
                    if (repeat++ >= 10) {
                        throw new ApplicationException ("尝试处理并发冲突次数超过10次。");
                    }
                }
            }
        }

        /// <summary>
        /// 保存修改
        /// <para>解决乐观并发冲突，重新加载数据库中的数据覆盖提交数据</para>
        /// <param name="cancellationToken">取消令牌</param>
        /// </summary>
        async Task SaveChangesReloadAsync (CancellationToken cancellationToken = default) {
            int repeat = 0;
            while (true) {
                try {
                    await base.SaveChangesAsync (cancellationToken);
                    break;
                } catch (DbUpdateConcurrencyException ex) {
                    foreach (var entry in ex.Entries) {
                        if (entry.Entity is TEntity) {
                            ex.Entries.Single ().Reload ();
                        } else {
                            throw new NotSupportedException (
                                "Don't know how to handle concurrency conflicts for " +
                                entry.Metadata.Name);
                        }
                    }
                    if (repeat++ >= 10) {
                        throw new ApplicationException ("尝试处理并发冲突次数超过10次。");
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定实体
        /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
        /// </summary>
        /// <param name="wheres">指定条件</param>
        /// <param name="tracking">是否追踪，追踪数据可进行修改、删除，不追踪提升查询性能</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TEntity> Get (Expression<Func<TEntity, bool>> wheres, bool tracking = false) {
            IQueryable<TEntity> iqueryable = tracking ? DbEntity.AsTracking () : DbEntity.AsNoTracking ();
            iqueryable = wheres == null ? iqueryable : iqueryable.Where (wheres);
            return iqueryable;
        }

        /// <summary>
        /// 获取指定实体
        /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
        /// </summary>
        /// <param name="wheres">指定条件</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TResult> Get<TResult> (Expression<Func<TEntity, bool>> wheres, Expression<Func<TEntity, TResult>> selector) {
            IQueryable<TEntity> iqueryable = DbEntity.AsNoTracking ();
            iqueryable = wheres == null ? iqueryable : iqueryable.Where (wheres);
            return iqueryable.Select (selector);
        }

        /// <summary>
        /// 获取指定实体
        /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="tracking">是否追踪，追踪数据可进行修改、删除，不追踪提升查询性能</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TEntity> Get (IANSHSpecification<TEntity> specification = null, bool tracking = false) {
            IQueryable<TEntity> iqueryable = tracking ? DbEntity.AsTracking () : DbEntity.AsNoTracking ();
            iqueryable = iqueryable.SetSpecification (specification);
            return iqueryable;
        }

        /// <summary>
        /// 获取指定实体
        /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="selector">返回指定实体类型</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TResult> Get<TResult> (IANSHSpecification<TEntity> specification, Expression<Func<TEntity, TResult>> selector) {
            IQueryable<TEntity> iqueryable = DbEntity.AsNoTracking ();
            iqueryable = iqueryable.SetSpecification (specification);
            return iqueryable.Select (selector);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="tSql">sql查询语句</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TEntity> FromSqlInterpolated (FormattableString tSql) => DbEntity.FromSql (tSql);
    }
}