//-----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Implement
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Interfaces;

  /// <summary>
  /// Class UserService.
  /// </summary>
  public class UserService : IUserService
  {
    /// <summary>
    /// UnitOfWork
    /// </summary>
    private IUnitOfWork<TemplateDbContext> unitOfWork = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <exception cref="System.ArgumentNullException">unitOfWork</exception>
    /// <exception cref="ArgumentNullException">dataContext</exception>
    public UserService(IUnitOfWork<TemplateDbContext> unitOfWork)
    {
      if (unitOfWork == null)
      {
        throw new ArgumentNullException("unitOfWork");
      }

      this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Determines whether this instance [can insert email] the specified email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns><c>true</c> if this instance [can insert email] the specified email; otherwise, <c>false</c>.</returns>
    public bool CanInsertEmail(string email)
    {
      bool canInsert = false;

      var query = this.unitOfWork.UserRepository.FindBy(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));

      if (query == null)
      {
        // no data found.
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Determines whether this instance [can insert user name] the specified user name.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns><c>true</c> if this instance [can insert user name] the specified user name; otherwise, <c>false</c>.</returns>
    public bool CanInsertUserName(string userName)
    {
      bool canInsert = false;

      var query = this.unitOfWork.UserRepository.FindBy(x => x.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase));

      if (query == null)
      {
        // no data found.
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Deletes the user by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <exception cref="System.InvalidOperationException">Due user not found.</exception>
    public void DeleteById(string id)
    {
      var query = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

      if (query != null)
      {
        this.unitOfWork.UserRepository.Delete(query);
        this.unitOfWork.Commit();
      }
      else
      {
        throw new InvalidOperationException(string.Format("Usert not found with the provided Id, Id provided: {0}", id));
      }
    }

    /// <summary>
    /// Exists the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public bool Exist(string id)
    {
      bool exists = false;

      var query = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

      if (query != null)
      {
        exists = true;
      }

      return exists;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>List of type <see cref="AspNetUser" />.</returns>
    public IEnumerable<AspNetUser> GetAll()
    {
      var users = this.unitOfWork.UserRepository.GetAll().ToList();
      List<AspNetUser> aspNetUsers = new List<AspNetUser>();
      foreach (var user in users)
      {
        // roles are null... dont know why, will do it the hard way.
        var aspNetUser = new AspNetUser
        {
          Id = new Guid(user.Id),
          Name = user.Name,
          UserName = user.UserName,
          Roles = this.GetRolesString(user.Id),
          Email = user.Email
        };

        aspNetUsers.Add(aspNetUser);
      }

      return aspNetUsers;
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole"/>.</returns>
    public IEnumerable<IdentityRole> GetAllRoles()
    {
      return this.unitOfWork.RoleRepository.GetAll().ToList();
    }

    /// <summary>
    /// Gets the roles by user identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>IEnumerable&lt;IdentityRole&gt;.</returns>
    public IEnumerable<IdentityRole> GetRolesByUserId(string id)
    {
      var query = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

      if (query == null)
      {
        throw new InvalidOperationException(string.Format("User not found with the provided id, id provided: {0}", id));
      }

      List<IdentityRole> userRoles = new List<IdentityRole>();
      var roles = this.unitOfWork.UseRolesRepository.FindManyBy(x => x.UserId.Equals(id, StringComparison.CurrentCultureIgnoreCase));

      foreach (var role in roles)
      {
        var userRole = this.unitOfWork.RoleRepository.FindBy(x => x.Id.Equals(role.RoleId, StringComparison.CurrentCultureIgnoreCase));
        userRoles.Add(userRole);
      }

      return userRoles;
    }

    /// <summary>
    /// Gets the user by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>User <see cref="ApplicationUser"/>.</returns>
    public ApplicationUser GetUserById(string id)
    {
      var query = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));

      if (query != null)
      {
        return query;
      }
      else
      {
        throw new InvalidOperationException(string.Format("User not found with the provided Id, provided Id: {0}", id));
      }
    }

    /// <summary>
    /// Gets the user roles by user identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>List of type <see cref="IdentityUserRole{TKey}" />.</returns>
    public IEnumerable<IdentityUserRole<string>> GetUserRolesByUserId(string id)
    {
      return this.unitOfWork.UseRolesRepository.FindManyBy(x => x.UserId.Equals(id, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }

    /// <summary>
    /// Inserts a new user.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="name">The name.</param>
    /// <param name="roleIds">The role ids.</param>
    public void Insert(string email, string userName, string name, IEnumerable<string> roleIds)
    {
      // create new User
      ApplicationUser newUser = new ApplicationUser
      {
        // set data.
        Name = name,
        UserName = userName,
        NormalizedUserName = userName,
        Email = email,
        NormalizedEmail = email,
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = Guid.NewGuid().ToString()
      };

      // password by default.
      var password = new PasswordHasher<ApplicationUser>();
      var hashed = password.HashPassword(newUser, "1122334455");
      newUser.PasswordHash = hashed;

      // attach to context.
      // this.dataContext.Users.Add(newUser);
      this.unitOfWork.UserRepository.Insert(newUser);

      var allUserRoles = this.unitOfWork.UseRolesRepository.GetAll().ToList();

      // query for the selected roles by the id and then add to the repository.
      foreach (var roleId in roleIds)
      {
        var role = this.unitOfWork.RoleRepository.FindBy(x => x.Id.Equals(roleId, StringComparison.CurrentCultureIgnoreCase));

        if (role != null)
        {
          // add to user roles.
          this.unitOfWork.UseRolesRepository.Insert(new IdentityUserRole<string> { RoleId = role.Id, UserId = newUser.Id });
        }
      }

      // save changes made to AspNetUsers table and AspNetUserRoles
      this.unitOfWork.Commit();
    }

    /// <summary>
    /// Updates the user information.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="name">The name.</param>
    public void UpdateUserInfo(string userId, string name)
    {
      var user = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(userId, StringComparison.CurrentCultureIgnoreCase));

      if (user != null)
      {
        user.Name = name;
        this.unitOfWork.UserRepository.Update(user);
        this.unitOfWork.Commit();
      }
      else
      {
        throw new InvalidOperationException(string.Format("User not found with the provided Id, provided Id: {0}", userId));
      }
    }

    /// <summary>
    /// Updates the user roles.
    /// </summary>
    /// <param name="newRolesToInsert">The new roles to insert.</param>
    /// <param name="rolesToDelete">The roles to delete.</param>
    public void UpdateUserRoles(List<IdentityUserRole<string>> newRolesToInsert, List<IdentityUserRole<string>> rolesToDelete)
    {
      foreach (var newRole in newRolesToInsert)
      {
        this.unitOfWork.UseRolesRepository.Insert(newRole);
      }

      foreach (var roleToDelete in rolesToDelete)
      {
        var useRoleFromDb = this.unitOfWork.UseRolesRepository.FindBy(x => x.RoleId.Equals(roleToDelete.RoleId) && x.UserId.Equals(roleToDelete.UserId));
        this.unitOfWork.UseRolesRepository.Delete(useRoleFromDb);
      }

      // save changes.
      this.unitOfWork.Commit();
    }

    /// <summary>
    /// Gets the roles string.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>System.String.</returns>
    private string GetRolesString(string userId)
    {
      List<string> roleNames = new List<string>();

      // query all the UserRoles.
      var userRoles = this.unitOfWork.UseRolesRepository.GetAll().ToList();

      foreach (var userRole in userRoles)
      {
        if (userRole.UserId.Equals(userId))
        {
          // if the UserRoles is equal...
          var role = this.unitOfWork.RoleRepository.FindBy(x => x.Id.Equals(userRole.RoleId));

          // finally...
          roleNames.Add(role.Name);
        }
      }

      return string.Join(", ", roleNames);
    }
  }
}