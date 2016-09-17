namespace TemplateCore.Service.Interfaces
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Model;
  using System.Collections.Generic;

  /// <summary>
  /// Interface IUserService
  /// </summary>
  public interface IUserService
  {
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>List of type <see cref="AspNetUser"/>.</returns>
    IEnumerable<AspNetUser> GetAll();

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole"/>.</returns>
    IEnumerable<IdentityRole> GetAllRoles();

    /// <summary>
    /// Determines whether this instance [can insert user name] the specified user name.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns><c>true</c> if this instance [can insert user name] the specified user name; otherwise, <c>false</c>.</returns>
    bool CanInsertUserName(string userName);

    /// <summary>
    /// Determines whether this instance [can insert email] the specified email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns><c>true</c> if this instance [can insert email] the specified email; otherwise, <c>false</c>.</returns>
    bool CanInsertEmail(string email);

    /// <summary>
    /// Inserts the specified email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="name">The name.</param>
    /// <param name="rolesIds">The roles ids.</param>
    void Insert(string email, string userName, string name, IEnumerable<string> rolesIds);

    /// <summary>
    /// Exists the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    bool Exist(string id);

    /// <summary>
    /// Deletes the user by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    void DeleteById(string id);

    /// <summary>
    /// Gets the user by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>ApplicationUser.</returns>
    ApplicationUser GetUserById(string id);

    /// <summary>
    /// Gets the roles by user identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>List of type <see cref="IdentityRole"/> .</returns>
    IEnumerable<IdentityRole> GetRolesByUserId(string id);

    /// <summary>
    /// Gets the user roles by user identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>List of type <see cref="IdentityUserRole<string>"/> .</returns>
    IEnumerable<IdentityUserRole<string>> GetUserRolesByUserId(string id);

    /// <summary>
    /// Updates the user roles.
    /// </summary>
    /// <param name="newRolesToInsert">The new roles to insert.</param>
    /// <param name="rolesToDelete">The roles to delete.</param>
    /// <param name="newName">The new name.</param>
    void UpdateUserRoles(List<IdentityUserRole<string>> newRolesToInsert, List<IdentityUserRole<string>> rolesToDelete);

    /// <summary>
    /// Updates the user information.
    /// </summary>
    /// <param name="name">The name.</param>
    void UpdateUserInfo(string userId, string name);
  }
}
