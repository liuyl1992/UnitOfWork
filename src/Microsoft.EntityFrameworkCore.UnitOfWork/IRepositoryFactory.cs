// Copyright (c) Arch team. All rights reserved.

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 仓储工厂
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 获取指定实体的仓储对象
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
