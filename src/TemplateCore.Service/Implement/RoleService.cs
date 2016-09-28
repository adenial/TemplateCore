//-----------------------------------------------------------------------
// <copyright file="RoleService.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Implement
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Security.Claims;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Interfaces;

  /// <summary>
  /// Class RoleService.
  /// </summary>
  public class RoleService : IRoleService
  {
    /// <summary>
    /// The unit of work
    /// </summary>
    private IUnitOfWork<TemplateDbContext> unitOfWork = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <exception cref="System.ArgumentNullException">unitOfWork</exception>
    public RoleService(IUnitOfWork<TemplateDbContext> unitOfWork)
    {
      if (unitOfWork == null)
      {
        throw new ArgumentNullException("unitOfWork");
      }

      this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Determines whether this instance can insert the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if this instance can insert the specified name; otherwise, <c>false</c>.</returns>
    public bool CanInsert(string name)
    {
      bool canInsert = false;

      var query = this.unitOfWork.RoleRepository.FindBy(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

      if (query == null)
      {
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Deletes a role with the specified name.
    /// </summary>
    /// <param name="name">The name of the role to delete.</param>
    public void DeleteRoleByName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentNullException("name");
      }

      var query = this.unitOfWork.RoleRepository.FindBy(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

      if (query == null)
      {
        throw new ArgumentNullException(string.Format("Role not found with the provided name, provided name: {0}", name));
      }

      this.unitOfWork.RoleRepository.Delete(query);
      this.unitOfWork.Commit();
    }

    /// <summary>
    /// Get all the role names.
    /// </summary>
    /// <returns>List of type <see cref="string" />.</returns>
    public IEnumerable<string> GetAllRoleNames()
    {
      return this.unitOfWork.RoleRepository.GetAll().ToList().Select(x => x.Name).ToList();

      /*var roleStore = new RoleStore<IdentityRole>(this.context);

      // list of role names.
      var roles = roleStore.Roles.ToList().Select(x => x.Name).ToList();*/

      // return roles;
    }

    /// <summary>
    /// Gets the claims.
    /// </summary>
    /// <returns>Dictionary of type string, Claim.</returns>
    public Dictionary<string, Claim> GetClaims()
    {
      return new Dictionary<string, Claim>()
      {
        // user actions.
        { "View Administrator Menu", new Claim(CustomClaimTypes.Permission, "Administrator.Menu") },
        { "View Users", new Claim(CustomClaimTypes.Permission, "Users.List") },
        { "Create Users", new Claim(CustomClaimTypes.Permission, "Users.Create") },
        { "Update Users", new Claim(CustomClaimTypes.Permission, "Users.Update") },
        { "Delete Users", new Claim(CustomClaimTypes.Permission, "Users.Delete") }
      };
    }

    /// <summary>
    /// Inserts a new role with the specified name
    /// </summary>
    /// <param name="name">The name of the role to Insert.</param>
    /// <param name="claims">The claims.</param>
    public void Insert(string name, IEnumerable<Claim> claims)
    {
      var newIdentityRole = new IdentityRole { Name = name, NormalizedName = name };

      this.unitOfWork.RoleRepository.Insert(newIdentityRole);

      foreach (var claim in claims)
      {
        this.unitOfWork.RoleClaimsRepository.Insert(new IdentityRoleClaim<string> { RoleId = newIdentityRole.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
      }

      this.unitOfWork.Commit();
    }
  }
}