//-----------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Without name">
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
  /// Interface IUnitOfWork
  /// </summary>
  /// <typeparam name="U">Database Context</typeparam>
  public interface IUnitOfWork<U>
    where U : DbContext, IDisposable
  {
    /// <summary>
    /// Gets the role repository.
    /// </summary>
    /// <value>The role repository.</value>
    IRepository<IdentityRole> RoleRepository { get; }

    /// <summary>
    /// Gets the use roles repository.
    /// </summary>
    /// <value>The use roles repository.</value>
    IRepository<IdentityUserRole<string>> UseRolesRepository { get; }

    /// <summary>
    /// Gets the user repository.
    /// </summary>
    /// <value>The user repository.</value>
    IRepository<ApplicationUser> UserRepository { get; }

    /// <summary>
    /// Saves all pending changes
    /// </summary>
    /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
    int Commit();
  }
}