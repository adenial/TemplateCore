namespace TemplateCore.Repository
{
  using Microsoft.EntityFrameworkCore;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Threading.Tasks;

  /// <summary>
  /// Class Repository.
  /// </summary>
  /// <typeparam name="TEntity">The type of the t entity.</typeparam>
  public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
  {
    #region Private Fields

    /// <summary>
    /// The data context
    /// </summary>
    private readonly DbContext dataContext;

    #endregion Private Fields

    #region Private Properties

    /// <summary>
    /// Gets the dbset.
    /// </summary>
    /// <value>The dbset.</value>
    private DbSet<TEntity> Dbset
    {
      get { return this.dataContext.Set<TEntity>(); }
    }

    #endregion Private Properties

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public Repository(DbContext context)
    {
      this.dataContext = context;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Delete the specified entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(TEntity entity)
    {
      this.Dbset.Remove(entity);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      if (this.dataContext != null)
      {
        this.dataContext.Dispose();
      }
    }

    /// <summary>
    /// Query by expression.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Object of the TEntity class.</returns>
    public TEntity FindBy(Expression<Func<TEntity, bool>> where)
    {
      return this.Dbset.Where(where).FirstOrDefault();
    }

    /// <summary>
    /// find by as an asynchronous operation.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Object of the TEntity class.</returns>
    public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> where)
    {
      return await Dbset.Where(where).FirstOrDefaultAsync();
    }

    /// <summary>
    /// find by many as an asynchronous operation.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Enumerable of the TEntity class.</returns>
    public async Task<IEnumerable<TEntity>> FindManyByAsync(Expression<Func<TEntity, bool>> where)
    {
      return await Dbset.Where(where).ToListAsync();
    }

    /// <summary>
    /// Query by expression.
    /// </summary>
    /// <param name="where">The where.</param>
    /// <returns>Enumerable of the TEntity class.</returns>
    public IEnumerable<TEntity> FindManyBy(Expression<Func<TEntity, bool>> where)
    {
      return this.Dbset.Where(where).ToList();
    }

    /// <summary>
    /// Get all rows.
    /// </summary>
    /// <returns>Enumerable of the TEntity class.</returns>
    public IEnumerable<TEntity> GetAll()
    {
      return this.Dbset.AsEnumerable();
    }

    /// <summary>
    /// get all as an asynchronous operation.
    /// </summary>
    /// <returns>Task{List{`0}}.</returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
      return await Dbset.ToListAsync();
    }

    /// <summary>
    /// Insert the entity.
    /// </summary>
    /// <param name="entity">The new entity to insert.</param>
    /// <exception cref="System.ArgumentNullException">entity</exception>
    public void Insert(TEntity entity)
    {
      if (entity != null)
      {
        this.Dbset.Add(entity);
      }
      else
      {
        throw new ArgumentNullException("entity");
      }
    }

    /// <summary>
    /// Update the entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(TEntity entity)
    {
      this.Dbset.Attach(entity);
      this.dataContext.Entry(entity).State = EntityState.Modified;
    }

    #endregion Public Methods
  }
}