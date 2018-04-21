// Copyright (c) Arch team. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 修改数据库名称. 前提是数据库在同一台主机. NOTE: 仅仅适用于用Mysql
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <remarks>
        /// 这仅用于支持同一模型中的多个数据库。这需要数据库在同一台机器上。
        /// </remarks>
        void ChangeDatabase(string database);

        /// <summary>
        ///获取指定仓储对象 
        /// </summary>
        /// <typeparam name="TEntity">Entity类型（对应于表名）</typeparam>
        /// <returns>返回此仓储对象实例</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// 将此上下文中的所有更改保存到数据库。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录。</param>
        /// <returns>写入数据库执行成功的状态实体的数量。</returns>
        int SaveChanges(bool ensureAutoHistory = false);

        /// <summary>
        ///异步保存此次上下文对数据库所做的所有更改。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录</param>
        /// <returns>A <see cref="Task{TResult}"/>这表示异步保存操.任务结果包含写入数据库的状态实体的数量。</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        /// <summary>
        /// 执行指定的原生SQL指令
        /// </summary>
        /// <param name="sql">原生SQL</param>
        /// <param name="parameters">SQL参数</param>
        /// <returns>写入数据库执行成功的状态实体的数量。</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 使用原生SQL查找指定实体的数据 
        /// </summary>
        /// <param name="sql">原生SQL</param>
        /// <param name="parameters">参数.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> 其中包含满足原始SQL指定条件的元素。</returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

        /// <summary>
        /// 使用TrakGrap Api 附加分离到实体
        /// </summary>
        /// <param name="rootEntity">根实体</param>
        /// <param name="callback">委托方法将对象的状态转换为实体进入状态。.</param>
        void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback);
    }
}
