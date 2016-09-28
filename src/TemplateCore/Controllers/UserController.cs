//-----------------------------------------------------------------------
// <copyright file="UserController.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using TemplateCore.Model;
  using TemplateCore.Service.Interfaces;
  using TemplateCore.ViewModels.User;

  /// <summary>
  /// Class UserController.
  /// </summary>
  [Authorize(Policy = "View Administrator Menu")]
  public class UserController : Controller
  {
    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private IStringLocalizer<UserController> localizer = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController" /> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="localizer">The localizer.</param>
    /// <exception cref="System.ArgumentNullException">userService</exception>
    public UserController(IUserService userService, IStringLocalizer<UserController> localizer)
    {
      if (userService == null)
      {
        throw new ArgumentNullException("userService");
      }

      if (localizer == null)
      {
        throw new ArgumentNullException("localizer");
      }

      this.localizer = localizer;
      this.userService = userService;
    }

    /// <summary>
    /// Create User Action
    /// </summary>
    /// <returns>View to create a new User</returns>
    //[Authorize(Roles = "Administrator")]
    [Authorize(Policy = "Create Users")]
    public IActionResult Create()
    {
      // create view model.
      var model = new UserCreateViewModel();

      var roles = this.userService.GetAllRoles();

      // load the roles. id and name.
      var rolesViewmodel = this.GetRolesForViewModel(roles);

      model.Roles = rolesViewmodel;

      // create view and return the model.
      return this.View(model);
    }

    /// <summary>
    /// Post Action of Create User
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Redirect to Index or return View with ModelErrors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Create Users")]
    public IActionResult Create(UserCreateViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        bool canInsert = false;

        // validate if user does not exists with provided username.
        canInsert = this.userService.CanInsertUserName(model.UserName);
        if (canInsert)
        {
          // validate if user does not exists with provided email.
          canInsert = this.userService.CanInsertEmail(model.Email);

          if (canInsert)
          {
            // get all the selected roles from model.Roles
            List<UserRoleCreateViewModel> selectedRoles = model.Roles.Where(x => x.Check == true).ToList();
            IEnumerable<string> rolesIds = selectedRoles.Select(x => x.Id).ToList();

            // if there are no more validations insert.
            this.userService.Insert(model.Email, model.UserName, model.Name, rolesIds);
            return this.RedirectToAction("Index");
          }
          else
          {
            this.ModelState.AddModelError("Email", this.localizer["There's already a user with the provided email."]);
            var roles = this.userService.GetAllRoles();

            // load the roles. id and name.
            var rolesViewmodel = this.GetRolesForViewModel(roles);
            model.Roles = rolesViewmodel;
            return this.View(model);
          }
        }
        else
        {
          this.ModelState.AddModelError("UserName", this.localizer["There's already a user with the provided username."]);
          var roles = this.userService.GetAllRoles();

          // load the roles. id and name.
          var rolesViewmodel = this.GetRolesForViewModel(roles);
          model.Roles = rolesViewmodel;

          return this.View(model);
        }
      }
      else
      {
        return this.View(model);
      }
    }

    /// <summary>
    /// Deletes the user by its Id
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Redirects to Index Page or HttpNotFound (404).</returns>
    [Authorize(Policy = "Delete Users")]
    public IActionResult Delete(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
      {
        return this.NotFound();
      }

      bool userExists = this.userService.Exist(id);

      if (!userExists)
      {
        return this.NotFound();
      }
      else
      {
        // delete and redirect to index.
        this.userService.DeleteById(id);
        return this.RedirectToAction("Index");
      }
    }

    /// <summary>
    /// Edits the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>IActionResult.</returns>
    [Authorize(Policy = "Update Users")]
    public IActionResult Edit(string id)
    {
      // validate if string is not null or empty.
      if (string.IsNullOrWhiteSpace(id))
      {
        // return 404.
        return this.NotFound();
      }

      // otherwise search the user given the id.
      ApplicationUser user = null;

      try
      {
        user = this.userService.GetUserById(id);
      }
      catch (InvalidOperationException)
      {
        return this.NotFound();
      }

      // define which information can be updated, if username and email can be updated, validate at post that no user already has the new username or email.
      // id cannot be updated.
      var model = new UserEditViewModel { Id = user.Id, Name = user.Name };

      // roles are empty.... (why Identity.. why..)
      var userRoles = this.userService.GetRolesByUserId(id);
      var allRoles = this.userService.GetAllRoles();

      model.Roles = this.GetUserRolesForEditViewModel(userRoles, allRoles);

      return this.View(model);
    }

    /// <summary>
    /// Edits the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Redirects to Index or to present the errors due validation.</returns>
    [ValidateAntiForgeryToken]
    [HttpPost]
    [Authorize(Policy = "Update Users")]
    public IActionResult Edit(UserEditViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        // for now the name of the user can be updated too..

        /*the roles have to be updated:
        the model has the property Roles which specifiy the new roles (added or deleted) will have.
        user id model.Id*/

        // UserRoles<string, string> identity
        var userRoles = this.userService.GetUserRolesByUserId(model.Id);

        var selectedRoles = model.Roles.Where(x => x.Check == true).ToList();
        var unselectedRoles = model.Roles.Where(x => x.Check == false).ToList();

        List<IdentityUserRole<string>> rolesToDelete = new List<IdentityUserRole<string>>();
        List<IdentityUserRole<string>> newRolesToInsert = new List<IdentityUserRole<string>>();

        var roles = this.userService.GetAllRoles();

        // query against the universe of roles.
        foreach (var roleDb in roles)
        {
          foreach (var selectedRole in selectedRoles)
          {
            if (roleDb.Id.Equals(selectedRole.Id))
            {
              var query = userRoles.Where(x => x.RoleId.Equals(selectedRole.Id)).SingleOrDefault();

              if (query == null)
              {
                // add
                newRolesToInsert.Add(new IdentityUserRole<string> { UserId = model.Id, RoleId = selectedRole.Id });
              }
            }
          }
        }

        // roles to delete.
        foreach (var userRole in userRoles)
        {
          foreach (var unselectedRole in unselectedRoles)
          {
            if (userRole.RoleId.Equals(unselectedRole.Id))
            {
              // roles to delete.
              rolesToDelete.Add(new IdentityUserRole<string> { UserId = model.Id, RoleId = userRole.RoleId });
            }
          }
        }

        this.userService.UpdateUserRoles(newRolesToInsert, rolesToDelete);
        this.userService.UpdateUserInfo(model.Id, model.Name);

        return this.RedirectToAction("Index");
      }
      else
      {
        return this.View(model);
      }
    }

    /// <summary>
    /// Index Page
    /// </summary>
    /// <returns>Index Page.</returns>
    //[Authorize(Roles = "Administrator")]
    [Authorize(Policy = "View Users")]
    public IActionResult Index()
    {
      var users = this.userService.GetAll();

      // create view model.
      var model = this.GetIndexModelFromUsers(users);

      return this.View(model);
    }

    /// <summary>
    /// Gets the index model from users.
    /// </summary>
    /// <param name="users">The users.</param>
    /// <returns>List <see cref="UserIndexViewModel"/>.</returns>
    private IEnumerable<UserIndexViewModel> GetIndexModelFromUsers(IEnumerable<AspNetUser> users)
    {
      return users.Select(x => new UserIndexViewModel { Id = x.Id, UserName = x.UserName, Name = x.Name, Roles = x.Roles, Email = x.Email }).ToList();
    }

    /// <summary>
    /// Gets the roles for view model.
    /// </summary>
    /// <param name="roles">The roles.</param>
    /// <returns>List of type <see cref="UserRoleCreateViewModel"/>.</returns>
    private IEnumerable<UserRoleCreateViewModel> GetRolesForViewModel(IEnumerable<IdentityRole> roles)
    {
      return roles.Select(x => new UserRoleCreateViewModel { Id = x.Id, Name = x.Name }).ToList();
    }

    /// <summary>
    /// Gets the user roles for edit view model.
    /// </summary>
    /// <param name="userRoles">The user roles.</param>
    /// <param name="roles">The roles.</param>
    /// <returns>List of type <see cref="UserRoleCreateViewModel"/>.</returns>
    private IEnumerable<UserRoleCreateViewModel> GetUserRolesForEditViewModel(IEnumerable<IdentityRole> userRoles, IEnumerable<IdentityRole> roles)
    {
      // this will create the view for all the roles... (the grid..)
      var rolesViewModel = roles.Select(x => new UserRoleCreateViewModel { Check = false, Id = x.Id, Name = x.Name }).ToList();

      // now preselect the property check = true against the roles the user already has
      foreach (var role in rolesViewModel)
      {
        foreach (var userRole in userRoles)
        {
          if (role.Id.Equals(userRole.Id))
          {
            role.Check = true;
          }
        }
      }

      return rolesViewModel;
    }
  }
}