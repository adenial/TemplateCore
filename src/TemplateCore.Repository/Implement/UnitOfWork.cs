//-----------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Repository
{
  using System;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Model;

  /// <summary>
  /// Class UnitOfWork.
  /// </summary>
  /// <typeparam name="TContext">The type of the t context.</typeparam>
  public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext>
    where TContext : DbContext, new()
  {
    /// <summary>
    /// The data context
    /// </summary>
    private DbContext dataContext = null;

    /// <summary>
    /// The roles repository
    /// Testing out the IdentityRole.
    /// </summary>
    private IRepository<IdentityRole> rolesRepository = null;

    /// <summary>
    /// The user repository
    /// </summary>
    private IRepository<ApplicationUser> userRepository = null;

    /// <summary>
    /// The user roles repository
    /// </summary>
    private IRepository<IdentityUserRole<string>> useRolesRepository = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UnitOfWork(TContext context)
    {
      this.dataContext = context;
    }

    /// <summary>
    /// Gets the user roles repository.
    /// </summary>
    /// <value>The user roles repository.</value>
    public IRepository<IdentityUserRole<string>> UseRolesRepository
    {
      get { return this.useRolesRepository ?? (this.useRolesRepository = new Repository<IdentityUserRole<string>>(this.dataContext)); }
    }

    /// <summary>
    /// Gets the role repository.
    /// </summary>
    /// <value>The role repository.</value>
    public IRepository<IdentityRole> RoleRepository
    {
      get { return this.rolesRepository ?? (this.rolesRepository = new Repository<IdentityRole>(this.dataContext)); }
    }

    /// <summary>
    /// Gets the user repository.
    /// </summary>
    /// <value>The user repository.</value>
    public IRepository<ApplicationUser> UserRepository
    {
      get { return this.userRepository ?? (this.userRepository = new Repository<ApplicationUser>(this.dataContext)); }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Saves all pending changes
    /// </summary>
    /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
    public int Commit()
    {
      return this.dataContext.SaveChanges();
    }

    /// <summary>
    /// Disposes all external resources.
    /// </summary>
    /// <param name="disposing">The dispose indicator.</param>
    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.dataContext != null)
        {
          this.dataContext.Dispose();
          this.dataContext = null;
        }
      }
    }
  }
}
