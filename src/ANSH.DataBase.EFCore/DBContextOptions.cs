using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ANSH.DataBase.Connection;
using ANSH.DDD.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ANSH.DataBase.EFCore {
    /// <summary>
    /// 操作实体基类
    /// </summary>
    /// <typeparam name="TEntity">实体模型</typeparam>
    public abstract class DBContextOptions<TEntity> : DBContext where TEntity : class, IDBEntity, new () {

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBContextOptions () : base () { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbconnection">数据库连接对象</param>
        /// <param name="loggers">日志记录</param>
        public DBContextOptions (DBConnection dbconnection, ILoggerFactory loggers) : base (dbconnection, loggers) { }

        /// <summary>
        /// 实体对象
        /// </summary>
        public DbSet<TEntity> DbEntity { get; set; }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TEntity[] Insert (params TEntity[] entity) {
            if ((entity ?? new TEntity[0]).Length > 0) {
                DbEntity.AddRange (entity);
                base.SaveChanges ();
                return entity;
            }
            return null;
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <returns>添加后的实体</returns>
        public virtual TEntity Insert (Action<TEntity> entity) {
            var insert = new TEntity ();
            entity?.Invoke (insert);
            if (insert == null) {
                return null;
            }
            DbEntity.Add (insert);
            base.SaveChanges ();
            return insert;
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="wheres">指定条件</param>
        public virtual void Update (Action<TEntity> entity, Expression<Func<TEntity, bool>> wheres = null) {
            Get (wheres, tracking : true).ToList ()?.ForEach ((item) => {
                entity (item);
                //Ignore (item);
                DbEntity.Update (item);
            });

            SaveChangesCoverage ();
        }

        /// <summary>
        /// 修改指定实体
        /// </summary>
        /// <param name="entity">需要修改的项</param>
        /// <param name="specification">规约</param>
        public virtual void Update (Action<TEntity> entity, IANSHSpecificationCommit<TEntity> specification) {
            Get (specification, tracking : true).ToList ().ForEach ((item) => {
                entity (item);
                //Ignore (item);
                DbEntity.Update (item);
            });

            if (specification?.CommitSaveChanges == CommitSaveChangesTypes.Reload) {
                SaveChangesReload ();
            } else {
                SaveChangesCoverage ();
            }
        }

        /// <summary>
        /// 删除指定实体
        /// </summary>
        /// <param name="wheres">指定条件</param>
        /// <param name="cascade">联级删除</param>
        public virtual void Delete (Expression<Func<TEntity, bool>> wheres = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> cascade = null) {
            var iqueryable = Get (wheres, tracking : true);
            if (cascade != null) iqueryable = cascade (iqueryable);
            DbEntity.RemoveRange (iqueryable);
            SaveChangesCoverage ();
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
                            entry.OriginalValues.SetValues (entry.GetDatabaseValues ());
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
        /// 忽略null值
        /// </summary>
        /// <param name="entity">模型</param>
        [Obsolete ("取消该方法", true)]
        public void Ignore (TEntity entity) {
            foreach (System.Reflection.PropertyInfo p in entity.GetType ().GetProperties ()) {

                if (p.IsDefined (typeof (RelationshipAttribute), true)) {
                    continue;
                }
                if (p.IsDefined (typeof (KeyAttribute), true)) {
                    continue;
                }

                if (p.GetValue (entity) != null) {
                    this.Entry<TEntity> (entity).Property (p.Name).IsModified = true;
                } else {
                    this.Entry<TEntity> (entity).Property (p.Name).IsModified = false;
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
        public virtual IQueryable<TEntity> Get (Expression<Func<TEntity, bool>> wheres = null, bool tracking = false) {
            IQueryable<TEntity> iqueryable = tracking ? DbEntity.AsTracking () : DbEntity.AsNoTracking ();
            iqueryable = wheres == null ? iqueryable : iqueryable.Where (wheres);
            return iqueryable;
        }

        /// <summary>
        /// 获取指定实体
        /// <para>注意：当需要对查询结果进行排序时，应先排序再进行分页</para>
        /// </summary>
        /// <param name="specification">规约</param>
        /// <param name="tracking">是否追踪，追踪数据可进行修改、删除，不追踪提升查询性能</param>
        /// <returns>返回满足条件的实体</returns>
        public virtual IQueryable<TEntity> Get (IANSHSpecification<TEntity> specification, bool tracking = false) {
            IQueryable<TEntity> iqueryable = tracking ? DbEntity.AsTracking () : DbEntity.AsNoTracking ();
            iqueryable = iqueryable.SetSpecification (specification);
            return iqueryable;
        }

        /// <summary>
        /// 执行SQL返回查询结果
        /// <para>一般用于查询结果</para>
        /// </summary>
        /// <param name="t_sql">sql查询语句</param>
        /// <param name="DBParameters">sql查询参数</param>
        /// <returns>返回查询结果，该结果必须是当前实体对象</returns>
        public virtual IQueryable<TEntity> SQLQuery (string t_sql, List<Connection.DBParameters> DBParameters = null) {
            return DbEntity.FromSql (t_sql, DBParameters?.Count == 0 ? null : DBParameters).AsNoTracking ();
        }

        /// <summary>
        /// 执行SQL返回受影响的行数
        /// <para>一般用于修改、删除</para>
        /// </summary>
        /// <param name="t_sql">sql查询语句</param>
        /// <param name="DBParameters">sql查询参数</param>
        /// <returns>返回受影响的行数</returns>
        public virtual int SQLNonQuery (string t_sql, List<Connection.DBParameters> DBParameters = null) {
            return DB_Connection.ExecuteSQLNonQuery (t_sql, DBParameters);
        }

        /// <summary>
        /// 执行SQL返回第一行第一列
        /// <para>一般用于添加</para>
        /// </summary>
        /// <param name="t_sql">sql查询语句</param>
        /// <param name="DBParameters">sql查询参数</param>
        /// <returns>返回第一行的第一列</returns>
        public virtual object SQLScalar (string t_sql, List<Connection.DBParameters> DBParameters = null) {
            return DB_Connection.ExecuteSQLScalar (t_sql, DBParameters);
        }

        /// <summary>
        /// 构建Page语句
        /// </summary>
        /// <param name="Tsql">查询表SQL</param>
        /// <param name="orderby">排序</param>
        /// <param name="output">输出参数</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页多少条</param>
        /// <returns>Page语句</returns>
        public string CreatePageTSQL (string Tsql, string orderby, out DBParameters output, int pageIndex = 1, int pageSize = 20) {
            string CountSql = $" SELECT @ROWSCOUNT=Count(*) FROM ({Tsql}) countpage";
            string PageSql = $@"
SELECT tbpage.* FROM ({Tsql}) tbpage
ORDER BY {orderby}
OFFSET {(pageIndex-1)*pageSize} ROWS
FETCH NEXT {pageSize} ROWS ONLY ; {CountSql}";
            output = new DBParameters () {
                ParameterName = "ROWSCOUNT",
                Size = 32,
                Direction = System.Data.ParameterDirection.Output,
                DbType = DbType.Int32
            };
            return PageSql.ToString ();
        }
    }
}