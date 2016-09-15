namespace TemplateCore.Repository
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  /// <summary>
  /// Interface IRepository
  /// </summary>
  /// <typeparam name="TEntity">The type of the t entity.</typeparam>
  public interface IRepository<TEntity> where TEntity : class
  {
    /// <summary>
    /// Get all rows.
    /// </summary>
    /// <returns>Enumerable of the TEntity class.</returns>
    IEnumerable<TEntity> GetAll();

    /// <summary>
    /// Query an object by the identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Object of the TEntity class.</returns>
    TEntity GetById(int id);

    /// <summary>
    /// Insert the entity.
    /// </summary>
    /// <param name="entity">The new entity to insert.</param>
    void Insert(TEntity entity);

    /// <summary>
    /// Update the entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Delete the specified entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Query by expression.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Object of the TEntity class.</returns>
    TEntity FindBy(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Query by expression.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Enumerable of the TEntity class.</returns>
    IEnumerable<TEntity> FindManyBy(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Gets all asynchronous.
    /// </summary>
    /// <returns>Task{List{`0}}.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Object of the TEntity class.</returns>
    Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> where);

    /// <summary>
    /// Gets the many asynchronous.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Enumerable of the TEntity class.</returns>
    Task<IEnumerable<TEntity>> FindByManyAsync(Expression<Func<TEntity, bool>> where);
  }
}
