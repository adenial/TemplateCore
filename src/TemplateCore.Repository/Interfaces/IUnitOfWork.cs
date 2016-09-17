namespace TemplateCore.Repository
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Model;
  using System;

  /// <summary>
  /// Interface IUnitOfWork
  /// </summary>
  /// <typeparam name="U"></typeparam>
  public interface IUnitOfWork<U> where U : DbContext, IDisposable
  {
    #region Public Properties

    IRepository<IdentityRole> RoleRepository { get; }

    IRepository<IdentityUserRole<string>> UseRolesRepository { get; }

    IRepository<ApplicationUser> UserRepository { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Saves all pending changes
    /// </summary>
    /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
    int Commit();

    #endregion Public Methods
  }
}