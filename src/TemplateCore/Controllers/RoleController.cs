namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Service;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using ViewModels.Role;

  /// <summary>
  /// Class AdminRoleController.
  /// </summary>
  [Authorize(Roles = "Administrator")]
  public class RoleController : Controller
  {
    private IRoleService roleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController" /> class.
    /// </summary>
    /// <param name="roleService">The role service.</param>
    public RoleController(IRoleService roleService)
    {
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

      // pass it to view.
      return View(model);
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

      this.roleService.DeleteRole(name);
      return this.RedirectToAction("Index");
    }

    /// <summary>
    /// Creates the specified model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(AdminRoleCreateViewModel model)
    {
      if (ModelState.IsValid)
      {
        // validate if new role can be inserted due name (avoid duplicated roles).
        bool canInsert = this.roleService.CanInsert(model.Name);

        if (canInsert)
        {
          // insert new role and redirect.
          await this.roleService.Insert(model.Name);
          return this.RedirectToAction("Index");
        }
        else
        {
          // add model error and return view with model.
          ModelState.AddModelError(string.Empty, "Ya existe un perfil con el nombre proporcionado.");
          return View(model);
        }
      }
      else
      {
        return this.View(model);
      }
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