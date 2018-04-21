// Copyright (c) Arch team. All rights reserved.

using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// 获取db context上下文.
        /// </summary>
        /// <returns>dbContext上下文实例</returns>
        TContext DbContext { get; }

        /// <summary>
        /// 异步将此上下文中的所有更改保存到具有分布式事务的数据库。
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> 如果保存更改，请确保自动记录更改历史记录</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/>这表示异步保存操作。任务结果包含写入数据库的状态实体的数量。.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);
    }
}
