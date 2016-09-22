namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Service.Interfaces;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using ViewModels.Role;

  /// <summary>
  /// Class AdminRoleController.
  /// </summary>
  [Authorize(Roles = "Administrator")]
  public class RoleController : Controller
  {
    #region Private Fields

    private IRoleService roleService;

    private IStringLocalizer<RoleController> localizer;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController" /> class.
    /// </summary>
    /// <param name="roleService">The role service.</param>
    /// <param name="localizer">The localizer.</param>
    public RoleController(IRoleService roleService, IStringLocalizer<RoleController> localizer)
    {
      if (roleService == null)
      {
        throw new ArgumentNullException("roleService");
      }

      if (localizer == null)
      {
        throw new ArgumentNullException("localizer");
      }

      this.localizer = localizer;
      this.roleService = roleService;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
      // create model.
      var model = new AdminRoleCreateViewModel();

      // pass it to view.
      return View(model);
    }

    /// <summary>
    /// Creates the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public IActionResult Create(AdminRoleCreateViewModel model)
    {
      if (ModelState.IsValid)
      {
        // validate if new role can be inserted due name (avoid duplicated roles).
        bool canInsert = this.roleService.CanInsert(model.Name);

        if (canInsert)
        {
          // insert new role and redirect.
          this.roleService.Insert(model.Name);
          return this.RedirectToAction("Index");
        }
        else
        {
          // add model error and return view with model.
          ModelState.AddModelError(string.Empty, this.localizer["There's already a role with the provided name."]);
          return View(model);
        }
      }
      else
      {
        return this.View(model);
      }
    }

    /// <summary>
    /// Deletes the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>IActionResult.</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Delete(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        // create custom page for not found.
        return this.NotFound();
      }

      // query the role given the name.

      this.roleService.DeleteRoleByName(name);
      return this.RedirectToAction("Index");
    }

    /// <summary>
    /// Indexes this instance.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Index()
    {
      var roleNames = this.roleService.GetAllRoleNames();
      var model = GetIndexViewModelFromRoles(roleNames);
      return View(model);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Gets the view model of the Index Action.
    /// </summary>
    /// <param name="roleNames">The role names.</param>
    /// <returns>List of type <see cref="AdminRoleIndexViewModel"/>.</returns>
    private IEnumerable<AdminRoleIndexViewModel> GetIndexViewModelFromRoles(IEnumerable<string> roleNames)
    {
      return roleNames.Select(x => new AdminRoleIndexViewModel { Name = x }).ToList();
    }

    #endregion Private Methods
  }
}