namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

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

    /// <summary>
    /// Create User Action
    /// </summary>
    /// <returns>View to create a new User</returns>
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
      // create view model.
      var model = new UserCreateViewModel();

      // create view and return the model.
      return this.View(model);
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
            // if there are no more validations insert.
            this.userService.Insert(model.Email, model.UserName, model.Name);
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
    /// Gets the index model from users.
    /// </summary>
    /// <param name="users">The users.</param>
    /// <returns>IEnumerable&lt;UserIndexViewModel&gt;.</returns>
    private IEnumerable<UserIndexViewModel> GetIndexModelFromUsers(IEnumerable<AspNetUser> users)
    {
      return users.Select(x => new UserIndexViewModel { Id = x.Id, UserName = x.UserName, Name = x.Name });
    }
  }
}