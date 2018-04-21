// Copyright (c) Arch team. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元默认实现
    /// </summary>
    /// <typeparam name="TContext"> DB上下文</typeparam>
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> repositories;

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="context">DB上下文</param>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <returns>DB上下文 <typeparamref name="TContext"/>.</returns>
        public TContext DbContext => _context;

        /// <summary>
        /// 修改数据库名称. 前提是数据库在同一台主机. NOTE: 仅仅适用于用Mysql
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <remarks>
        /// 这仅用于支持同一模型中的多个数据库。这需要数据库在同一台机器上。
        /// </remarks>
        public void ChangeDatabase(string database)
        {
            var connection = _context.Database.GetDbConnection();
            if (connection.State.HasFlag(ConnectionState.Open))
            {
                connection.ChangeDatabase(database);
            }
            else
            {
                var connectionString = Regex.Replace(connection.ConnectionString.Replace(" ", ""), @"(?<=[Dd]atabase=)\w+(?=;)", database, RegexOptions.Singleline);
                connection.ConnectionString = connectionString;
            }

            // 以下代码仅在Mysql中工作
            var items = _context.Model.GetEntityTypes();
            foreach (var item in items)
            {
                if (item.Relational() is RelationalEntityTypeAnnotations extensions)
                {
                    extensions.Schema = database;
                }
            }
        }

        /// <summary>
        ///获取指定仓储对象 
        /// </summary>
        /// <typeparam name="TEntity">Entity类型（对应于表名）</typeparam>
        /// <returns>返回此仓储对象实例</returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// 执行指定的原生SQL指令
        /// </summary>
        /// <param name="sql">原生SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>写入数据库执行成功的状态实体的数量。</returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlCommand(sql, parameters);

        /// <summary>
        /// 使用原生SQL查询出指定类型的集合<typeparamref name="TEntity" /> data.
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="sql">原生SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>An <see cref="IQueryable{T}" />返回满足指定条件的元素集合</returns>
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSql(sql, parameters);

        /// <summary>
        /// 将此上下文中的所有更改保存到数据库。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录。</param>
        /// <returns>写入数据库执行成功的状态实体的数量。</returns>
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                _context.EnsureAutoHistory();
            }

            return _context.SaveChanges();
        }

        /// <summary>
        ///异步保存此次上下文对数据库所做的所有更改。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录</param>
        /// <returns>A <see cref="Task{TResult}"/>这表示异步保存操.任务结果包含写入数据库的状态实体的数量。</returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                _context.EnsureAutoHistory();
            }

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 异步将此上下文中的所有更改保存到具有分布式事务的数据库。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/>这表示异步保存操作。任务结果包含写入数据库的状态实体的数量。.</returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
        {
            // TransactionScope将包含在.NETCore v2.0中。
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var count = 0;
                    foreach (var unitOfWork in unitOfWorks)
                    {
                        var uow = unitOfWork as UnitOfWork<DbContext>;
                        uow.DbContext.Database.UseTransaction(transaction.GetDbTransaction());
                        count += await uow.SaveChangesAsync(ensureAutoHistory);
                    }

                    count += await SaveChangesAsync(ensureAutoHistory);

                    transaction.Commit();

                    return count;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();

                    throw ex;
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            _context.ChangeTracker.TrackGraph(rootEntity, callback);
        }
    }
}
