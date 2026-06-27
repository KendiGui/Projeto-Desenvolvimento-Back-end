using System.Linq.Expressions;

namespace Core.Data
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        Task<TEntity?> GetByIdAsync(long id);

        /// <summary>
        /// Gets an entity with tracking disabled (useful for read-only operations)
        /// </summary>
        Task<TEntity?> GetByIdAsNoTrackingAsync(long id);

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<TEntity>> ListAsync();

        /// <summary>
        /// Gets all entities with tracking disabled
        /// </summary>
        Task<IEnumerable<TEntity>> ListAsNoTrackingAsync();

        /// <summary>
        /// Finds entities matching a predicate
        /// </summary>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds entities matching a predicate with tracking disabled
        /// </summary>
        Task<IEnumerable<TEntity>> FindAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds the first entity matching a predicate
        /// </summary>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds the first entity matching a predicate with tracking disabled
        /// </summary>
        Task<TEntity?> FirstOrDefaultAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Checks if any entity matches the predicate
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts entities matching a predicate
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts all entities
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates an entity
        /// </summary>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        Task DeleteAsync(long id);

        /// <summary>
        /// Deletes a specific entity
        /// </summary>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes multiple entities
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes entities matching a predicate
        /// </summary>
        Task DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
