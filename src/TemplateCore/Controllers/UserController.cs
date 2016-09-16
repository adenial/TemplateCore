namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc;
  using Model;
  using Service;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using ViewModels.User;

  /// <summary>
  /// Class UserController.
  /// </summary>
  [Authorize(Roles = "Administrator")]
  public class UserController : Controller
  {
    #region Private Fields

    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <exception cref="System.ArgumentNullException">userService</exception>
    public UserController(IUserService userService)
    {
      if (userService == null)
      {
        throw new ArgumentNullException("userService");
      }

      this.userService = userService;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Create User Action
    /// </summary>
    /// <returns>View to create a new User</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
      // create view model.
      var model = new UserCreateViewModel();

      var roles = this.userService.GetAllRoles();

      // load the roles. id and name.
      var rolesViewmodel = GetRolesForViewModel(roles);

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
    [Authorize(Roles = "Administrator")]
    public IActionResult Create(UserCreateViewModel model)
    {
      if (ModelState.IsValid)
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
            var selectedRoles = model.Roles.Where(x => x.Check == true);
            IEnumerable<string> rolesIds = selectedRoles.Select(x => x.Id).ToList();

            // if there are no more validations insert.
            this.userService.Insert(model.Email, model.UserName, model.Name, rolesIds);
            return RedirectToAction("Index");
          }
          else
          {
            ModelState.AddModelError(string.Empty, "Ya existe un usuario con el correo electrónico proporcionado.");
            return View(model);
          }
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Ya existe un usuario con el nombre de usuario proporcionado.");
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

    /// <summary>
    /// Deletes the user by its Id
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Redirects to Index Page or HttpNotFound (404).</returns>
    [Authorize(Roles = "Administrator")]
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
        return RedirectToAction("Index");
      }
    }

    /// <summary>
    /// Edits the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>IActionResult.</returns>
    public IActionResult Edit(string id)
    {
      // validate if string is not null or empty.
      if (string.IsNullOrWhiteSpace(id))
      {
        // return 404.
        return this.NotFound();
      }

      // otherwise search the user given the id.
      var query = this.userService.GetUserById(id);

      if (query == null)
      {
        // return 404
        return this.NotFound();
      }

      // define which information can be updated, if username and email can be updated, validate at post that no user already has the new username or email.
      // id cannot be updated.
      var model = new UserEditViewModel { Id = query.Id, Name = query.Name };

      // roles are empty.... (why Identity.. why..)
      var userRoles = this.userService.GetRolesByUserId(id);
      var allRoles = this.userService.GetAllRoles();

      model.Roles = GetUserRolesForEditViewModel(userRoles, allRoles);

      return this.View(model);
    }

    /// <summary>
    /// Edits the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Redirects to Index or to present the errors due validation.</returns>
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Edit(UserEditViewModel model)
    {
      if (ModelState.IsValid)
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

        return RedirectToAction("Index");
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
    [Authorize(Roles = "Administrator")]
    public IActionResult Index()
    {
      // query for the users.
      var users = this.userService.GetAll();

      // create view model.
      var model = GetIndexModelFromUsers(users);

      return View(model);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Gets the index model from users.
    /// </summary>
    /// <param name="users">The users.</param>
    /// <returns>IEnumerable&lt;UserIndexViewModel&gt;.</returns>
    private IEnumerable<UserIndexViewModel> GetIndexModelFromUsers(IEnumerable<AspNetUser> users)
    {
      return users.Select(x => new UserIndexViewModel { Id = x.Id, UserName = x.UserName, Name = x.Name, Roles = x.Roles });
    }

    /// <summary>
    /// Gets the roles for view model.
    /// </summary>
    /// <param name="roles">The roles.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;TemplateCore.ViewModels.User.UserRoleCreateViewModel&gt;.</returns>
    private IEnumerable<UserRoleCreateViewModel> GetRolesForViewModel(IEnumerable<IdentityRole> roles)
    {
      return roles.Select(x => new UserRoleCreateViewModel { Id = x.Id, Name = x.Name }).ToList();
    }

    /// <summary>
    /// Gets the user roles for edit view model.
    /// </summary>
    /// <param name="userRoles">The user roles.</param>
    /// <param name="roles">The roles.</param>
    /// <returns>List of type <see cref="UserRoleCreateViewModel"/> .</returns>
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

    #endregion Private Methods
  }
}