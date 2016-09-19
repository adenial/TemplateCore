namespace TemplateCore.Controllers
{
  using Core;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Microsoft.Extensions.Logging;
  using Model;
  using Service.Interfaces;
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

    /// <summary>
    /// The localizer
    /// </summary>
    private IStringLocalizer<UserController> localizer = null;

    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<UserController> logger = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController" /> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="localizer">The localizer.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">userService</exception>
    public UserController(IUserService userService, IStringLocalizer<UserController> localizer, ILogger<UserController> logger)
    {
      if (userService == null)
      {
        throw new ArgumentNullException("userService");
      }

      if (localizer == null)
      {
        throw new ArgumentNullException("localizer");
      }

      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      this.logger = logger;
      this.localizer = localizer;
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
            List<UserRoleCreateViewModel> selectedRoles = model.Roles.Where(x => x.Check == true).ToList();
            IEnumerable<string> rolesIds = selectedRoles.Select(x => x.Id).ToList();

            // if there are no more validations insert.
            this.userService.Insert(model.Email, model.UserName, model.Name, rolesIds);
            this.logger.LogInformation(LoggingEvents.INSERT, string.Format("The user {0}, created a new user.", HttpContext.User.Identity.Name));
            return RedirectToAction("Index");
          }
          else
          {
            ModelState.AddModelError("Email", this.localizer["There's already a user with the provided email."]);

            var roles = this.userService.GetAllRoles();
            // load the roles. id and name.
            var rolesViewmodel = GetRolesForViewModel(roles);
            model.Roles = rolesViewmodel;
            return this.View(model);
          }
        }
        else
        {
          ModelState.AddModelError("UserName", this.localizer["There's already a user with the provided username."]);
          var roles = this.userService.GetAllRoles();
          // load the roles. id and name.
          var rolesViewmodel = GetRolesForViewModel(roles);
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
        this.logger.LogInformation(LoggingEvents.DELETE, string.Format("The user {0} deleted the user with the Id: {1}", HttpContext.User.Identity.Name, id));
        return RedirectToAction("Index");
      }
    }

    /// <summary>
    /// Edits the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>IActionResult.</returns>
    [Authorize(Roles = "Administrator")]
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
    [Authorize(Roles = "Administrator")]
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

        this.logger.LogInformation(LoggingEvents.UPDATE, string.Format("The user {0} edited the user {1}", HttpContext.User.Identity.Name, model.Name));

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
      this.logger.LogInformation(LoggingEvents.LIST, "Listing all users");
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