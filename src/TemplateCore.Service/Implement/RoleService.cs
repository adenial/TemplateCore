namespace TemplateCore.Service.Implement
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Model;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using TemplateCore.Service.Interfaces;

  /// <summary>
  /// Class RoleService.
  /// </summary>
  public class RoleService : IRoleService
  {
    #region Private Fields

    private TemplateDbContext context;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public RoleService(TemplateDbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      this.context = context;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Determines whether this instance can insert the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if this instance can insert the specified name; otherwise, <c>false</c>.</returns>
    public bool CanInsert(string name)
    {
      bool canInsert = false;

      var roleStore = new RoleStore<IdentityRole>(this.context);

      if (!context.Roles.Any(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
      {
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Deletes a role with the specified name.
    /// </summary>
    /// <param name="name">The name of the role to delete.</param>
    /// <returns>Task.</returns>
    public void DeleteRole(string name)
    {
      //var roleStore = new RoleStore<IdentityRole>(this.context);

      //// test?
      //await roleStore.DeleteAsync(new IdentityRole { Name = name, NormalizedName = name }, new System.Threading.CancellationToken());
      var role = this.context.Roles.Where(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();

      this.context.Roles.Remove(role);
      this.context.SaveChanges();
    }

    /// <summary>
    /// Get all the role names.
    /// </summary>
    /// <returns>List of type <see cref="string" />.</returns>
    public IEnumerable<string> GetAllRoleNames()
    {
      var roleStore = new RoleStore<IdentityRole>(this.context);

      // list of role names.
      var roles = roleStore.Roles.ToList().Select(x => x.Name).ToList();

      return roles;
    }

    /// <summary>
    /// Inserts a new role with the specified name
    /// </summary>
    /// <param name="name">The name of the role to Insert.</param>
    public async Task Insert(string name)
    {
      var roleStore = new RoleStore<IdentityRole>(context);

      await roleStore.CreateAsync(new IdentityRole { Name = name, NormalizedName = name });

      // not needed
      // context.SaveChanges();
    }

    #endregion Public Methods
  }
}