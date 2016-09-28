//-----------------------------------------------------------------------
// <copyright file="RoleController.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Service.Interfaces;
  using TemplateCore.ViewModels.Role;
  using System.Security.Claims;

  /// <summary>
  /// Class AdminRoleController.
  /// </summary>
  [Authorize(Roles = "Administrator")]
  public class RoleController : Controller
  {
    /// <summary>
    /// The role service
    /// </summary>
    private IRoleService roleService;

    /// <summary>
    /// The localizer
    /// </summary>
    private IStringLocalizer<RoleController> localizer;

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

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
      // create model.
      var model = new AdminRoleCreateViewModel();

      model.Claims = this.GetClaims();

      // pass it to view.
      return this.View(model);
    }

    /// <summary>
    /// Gets the claims.
    /// </summary>
    /// <returns>List of type <see cref="RoleClaimCreateViewModel"/>.</returns>
    private IEnumerable<RoleClaimCreateViewModel> GetClaims()
    {
      var claims = this.roleService.GetClaims();

      if (claims != null)
      {
        return claims.Select(x => new RoleClaimCreateViewModel { Check = false, Value = x.Value.Value, Key = x.Key, Type = x.Value.Type }).ToList();
      }

      return null;
    }

    /// <summary>
    /// Action to create a new user.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Redirects to Index or returns view with model.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public IActionResult Create(AdminRoleCreateViewModel model)
    {
      if (this.ModelState.IsValid)
      {
        // validate if new role can be inserted due name (avoid duplicated roles).
        bool canInsert = this.roleService.CanInsert(model.Name);

        if (canInsert)
        {
          // create claims and insert too.
          var claims = model.Claims.Where(x => x.Check == true).Select(x => new Claim(x.Type, x.Value)).ToList();

          // insert new role and redirect.
          this.roleService.Insert(model.Name, claims);
          return this.RedirectToAction("Index");
        }
        else
        {
          // add model error and return view with model.
          this.ModelState.AddModelError(string.Empty, this.localizer["There's already a role with the provided name."]);
          return this.View(model);
        }
      }
      else
      {
        return this.View(model);
      }
    }

    /// <summary>
    /// Deletes the user with the provided name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>Redirects to Index or view not found.</returns>
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
    /// Index page.
    /// </summary>
    /// <returns>List role view.</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Index()
    {
      var roleNames = this.roleService.GetAllRoleNames();
      var model = this.GetIndexViewModelFromRoles(roleNames);
      return this.View(model);
    }

    /// <summary>
    /// Gets the view model of the Index Action.
    /// </summary>
    /// <param name="roleNames">The role names.</param>
    /// <returns>List of type <see cref="AdminRoleIndexViewModel"/>.</returns>
    private IEnumerable<AdminRoleIndexViewModel> GetIndexViewModelFromRoles(IEnumerable<string> roleNames)
    {
      return roleNames.Select(x => new AdminRoleIndexViewModel { Name = x }).ToList();
    }
  }
}