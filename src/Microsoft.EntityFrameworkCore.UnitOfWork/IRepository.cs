// Copyright (c) Arch team. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Defines the interfaces for generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 修改表名
        /// </summary>
        /// <param name="table"></param>
        /// <remarks>
        /// 这仅用于支持同一模型中的多个表。这需要相同数据库中的表。
        /// </remarks>
        void ChangeTable(string table);

        /// <summary>
        /// 查询出指定条件的分页集合。 此方法默认无跟踪查询。
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">禁用更改跟踪</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含指定条件的元素</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         int pageIndex = 0,
                                         int pageSize = 20,
                                         bool disableTracking = true);

        /// <summary>
        /// 异步 查询出指定条件的分页集合。 此方法默认无跟踪查询。
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">禁用更改跟踪</param>
        /// <param name="cancellationToken">终止线程标识</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含指定条件的元素</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                    int pageIndex = 0,
                                                    int pageSize = 20,
                                                    bool disableTracking = true,
                                                    CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 查询出指定条件的分页集合。 此方法默认无跟踪查询。
        /// </summary>
        /// <param name="selector">A function to test each element for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">禁用更改跟踪</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含指定条件的元素</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  int pageIndex = 0,
                                                  int pageSize = 20,
                                                  bool disableTracking = true) where TResult : class;

        /// <summary>
        /// 异步查询出指定条件的分页集合。 此方法默认无跟踪查询。
        /// </summary>
        /// <param name="selector">A function to test each element for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="disableTracking">禁用更改跟踪</param>
        /// <param name="cancellationToken">终止线程标识</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含指定条件的元素</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                             Expression<Func<TEntity, bool>> predicate = null,
                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                             int pageIndex = 0,
                                                             int pageSize = 20,
                                                             bool disableTracking = true,
                                                             CancellationToken cancellationToken = default(CancellationToken)) where TResult : class;

        /// <summary>
        /// 查询出指定条件表达式的单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disableTracking">禁用跟踪</param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                  bool disableTracking = true);

        /// <summary>
        /// 获取基于谓词、orderby委托的第一个或默认实体，并包括委托。此方法默认为只读、无跟踪查询。
        /// </summary>
        /// <param name="selector">投影选择器.</param>
        /// <param name="predicate">指定要满足的条件表达式.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">禁用更改跟踪. 默认禁用</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含满足指定条件的元素。</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                           Expression<Func<TEntity, bool>> predicate = null,
                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                           bool disableTracking = true);

        /// <summary>
        /// 异步获取基于谓词、orderby委托的第一个或默认实体，并包括委托。此方法默认为只读、无跟踪查询。
        /// </summary>
        /// <param name="selector">投影选择器.</param>
        /// <param name="predicate">指定要满足的条件表达式.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">禁用更改跟踪. 默认禁用</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含满足指定条件的元素。</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        /// 异步获取基于谓词、orderby委托的第一个或默认实体，并包括委托。此方法默认为只读、无跟踪查询。
        /// </summary>
        /// <param name="predicate">指定要满足的条件表达式.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties</param>
        /// <param name="disableTracking">禁用更改跟踪. 默认禁用</param>
        /// <returns>An <see cref="IPagedList{TEntity}"/>包含满足指定条件的元素。</returns>
        /// <remarks>Ex: 此方法默认为只读、无跟踪查询。.</remarks>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true);

        /// <summary>
        /// 使用原生SQL查找指定实体的数据 Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">原生SQL</param>
        /// <param name="parameters">参数.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> 其中包含满足原始SQL指定条件的元素。</returns>
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        /// <summary>
        /// 找到具有给定主键值的实体。如果找到，则附加到上下文并返回。如果没有找到实体，则返回null。
        /// </summary>
        /// <param name="keyValues">对应的实体的主键值</param>
        /// <returns>找到的实体或null.</returns>
        TEntity Find(params object[] keyValues);

        /// <summary>
        /// 异步找到具有给定主键值的实体。如果找到，则附加到上下文并返回。如果没有找到实体，则返回null。
        /// </summary>
        /// <param name="keyValues">对应的实体的主键值</param>
        /// <returns>A <see cref="Task{TEntity}" /> 异步查找操作返回找到的实体或null</returns>
        Task<TEntity> FindAsync(params object[] keyValues);

        /// <summary>
        /// 异步找到具有给定主键值的实体。如果找到，则附加到上下文并返回。如果没有找到实体，则返回null。
        /// </summary>
        /// <param name="keyValues">对应的实体的主键值。</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> 终止线程标识.</param>
        /// <returns>A <see cref="Task{TEntity}"/> 这表示异步查找操作。任务结果包含找到的实体或null。</returns>
        Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all entities. This method is not recommended
        /// </summary>
        /// <returns>The <see cref="IQueryable{TEntity}"/>.</returns>
        [Obsolete("This method is not recommended, please use GetPagedList or GetPagedListAsync methods")]
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取满足条件的数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// 插入一个新的实体
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// 插入一系列实体
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        void Insert(params TEntity[] entities);

        /// <summary>
        ///插入一系列实体
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        ///异步插入新的实体对象
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> 终止线程标识.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous insert operation.</returns>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 异步插入一系列实体
        /// </summary>
        /// <param name="entities">待插入实体集合</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous insert operation.</returns>
        Task InsertAsync(params TEntity[] entities);

        /// <summary>
        /// 异步插入一系列实体
        /// </summary>
        /// <param name="entities">待插入实体</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> 终止线程标识</param>
        /// <returns>A <see cref="Task"/>这是异步插入操作</returns>
        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 更新指定实体
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// 更新指定实体
        /// </summary>
        /// <param name="entities">待更新的实体集合.</param>
        void Update(params TEntity[] entities);

        /// <summary>
        /// 更新指定实体
        /// </summary>
        /// <param name="entities">待更新的实体集合.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// 通过指定主键删除实体
        /// </summary>
        /// <param name="id">主键值</param>
        void Delete(object id);

        /// <summary>
        /// 删除单个指定实体
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除多个指定实体
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(params TEntity[] entities);

        /// <summary>
        /// 删除多个指定实体
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<TEntity> entities);
    }
}
