namespace TemplateCore.Tests.Controllers.User
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Model;
  using Moq;
  using Service.Interfaces;
  using System;
  using System.Collections.Generic;
  using TemplateCore.Controllers;
  using ViewModels.User;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Edit of the class <see cref="UserController" />.
  /// </summary>
  public class Edit
  {
    #region Private Fields

    /// <summary>
    /// The controller
    /// </summary>
    private UserController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<UserController>> localizer = null;

    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    /// <summary>
    /// The administrator role identifier
    /// </summary>
    private string administratorRoleId = string.Empty;

    /// <summary>
    /// The payroll role identifier
    /// </summary>
    private string payrollRoleId = string.Empty;

    /// <summary>
    /// The reporter role identifier
    /// </summary>
    private string reporterRoleId = string.Empty;

    /// <summary>
    /// The user role identifier
    /// </summary>
    private string userRoleId = string.Empty;

    /// <summary>
    /// The user service
    /// </summary>
    private Mock<IUserService> userService = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Edit" /> class.
    /// </summary>
    public Edit()
    {
      this.userId = Guid.NewGuid().ToString();
      this.administratorRoleId = Guid.NewGuid().ToString();
      this.payrollRoleId = Guid.NewGuid().ToString();
      this.reporterRoleId = Guid.NewGuid().ToString();
      this.userRoleId = Guid.NewGuid().ToString();
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method Edit of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="NotFoundResult"/>.
    /// Test case when the provided user Id is empty or whitespace.
    /// </summary>
    [Fact]
    public void EditInvalidParameter()
    {
      // setup
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Edit(" ") as NotFoundResult;

      // assert
      Assert.IsType(typeof(NotFoundResult), result);
    }

    /// <summary>
    /// Tests the method Edit of the class <see cref="UserController"/>.
    /// Asserts the invoke of the method returns an object of the type <see cref="UserEditViewModel"/>.
    /// </summary>
    [Fact]
    public void EditOk()
    {
      // setup
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();

      this.userService.Setup(x => x.GetUserById(this.userId)).Returns(CreateUser());
      this.userService.Setup(x => x.GetRolesByUserId(this.userId)).Returns(GetRoles());
      this.userService.Setup(x => x.GetAllRoles()).Returns(GetAllRoles());
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = (this.controller.Edit(userId) as ViewResult).Model as UserEditViewModel;

      // assert
      Assert.IsType(typeof(UserEditViewModel), result);
    }

    /// <summary>
    /// Tests the method Edit of the class <see cref="UserController"/>
    /// Asserts the invoke of the method returns an instance of <see cref="NotFoundResult"/>
    /// Tests case when the user is not found with the provided Id
    /// </summary>
    [Fact]
    public void EditUserNotFound()
    {
      // setup
      this.userService = new Mock<IUserService>();
      this.userService.Setup(x => x.GetUserById(this.userId)).Throws(new InvalidOperationException("Test"));
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Edit(this.userId) as NotFoundResult;

      // assert
      Assert.IsType(typeof(NotFoundResult), result);
    }

    /// <summary>
    /// Tests the method Edit POST action of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="RedirectToActionResult"/>. 
    /// Happy Path 
    /// </summary>
    [Fact]
    public void EditPostOk()
    {
      // setup
      var model = new UserEditViewModel { Id = this.userId, Name = "User for tests purposes", Roles = GetRolesEditView() };
      this.userService = new Mock<IUserService>();
      this.userService.Setup(x => x.GetUserRolesByUserId(this.userId)).Returns(GetUserRolesPost());
      this.userService.Setup(x => x.GetAllRoles()).Returns(GetAllRolesPost());
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Edit(model) as RedirectToActionResult;

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }

    /// <summary>
    /// Tests the method Edit POST action of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="UserEditViewModel"/>.
    /// </summary>
    [Fact]
    public void EditPostInvalidModel()
    {
      // setup
      var model = new UserEditViewModel { Id = this.userId, Name = "User for tests purposes", Roles = GetRolesEditView() };
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.controller = new UserController(this.userService.Object, this.localizer.Object);
      this.controller.ModelState.AddModelError("", "Test");

      // action
      var result = (this.controller.Edit(model) as ViewResult).Model as UserEditViewModel;

      // assert
      Assert.IsType(typeof(UserEditViewModel), result);
    }

    /// <summary>
    /// Gets all roles post.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole"/>.</returns>
    private IEnumerable<IdentityRole> GetAllRolesPost()
    {
      return new List<IdentityRole>
      {
        new IdentityRole { Id = this.administratorRoleId, Name = "Administrator" },
        new IdentityRole { Id = this.payrollRoleId, Name = "Payroll"},
        new IdentityRole { Id = this.reporterRoleId, Name = "Execute Reports"},
        new IdentityRole { Id = this.userRoleId, Name = "User"}
      };
    }

    /// <summary>
    /// Gets the user roles post.
    /// </summary>
    /// <returns>List of type <see cref="IdentityUserRole<string>"/>.</returns>
    private IEnumerable<IdentityUserRole<string>> GetUserRolesPost()
    {
      return new List<IdentityUserRole<string>>
      {
        // roles the user "already" has.
        new IdentityUserRole<string> { UserId = this.userId, RoleId = this.userRoleId },
        new IdentityUserRole<string> { UserId = this.userId, RoleId = this.administratorRoleId },
        new IdentityUserRole<string> { UserId = this.userId, RoleId = this.payrollRoleId}

      };
    }

    /// <summary>
    /// Gets the roles edit view.
    /// </summary>
    /// <returns>List of type <see cref="UserRoleCreateViewModel"/>.</returns>
    private IEnumerable<UserRoleCreateViewModel> GetRolesEditView()
    {
      // the edited roles.
      return new List<UserRoleCreateViewModel>
      {
        new UserRoleCreateViewModel { Check = false, Id = this.administratorRoleId, Name = "Administrator" },
        new UserRoleCreateViewModel { Check = true, Id = this.payrollRoleId, Name = "Payroll" },
        new UserRoleCreateViewModel { Check = false, Id = this.reporterRoleId, Name = "Execute reports" },
        new UserRoleCreateViewModel { Check = true, Id = this.userRoleId, Name = "User" }
      };
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Creates the user.
    /// </summary>
    /// <returns>ApplicationUser.</returns>
    private ApplicationUser CreateUser()
    {
      return new ApplicationUser { Name = "User for Tests purposes", Id = this.userId };
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole" /></returns>
    private IEnumerable<IdentityRole> GetAllRoles()
    {
      return new List<IdentityRole>
      {
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role for tests purposes"},
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 2 for tests purposes"},
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 3 for tests purposes"},
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 4 for tests purposes"},
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 5 for tests purposes"},
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 6 for tests purposes"}
      };
    }

    /// <summary>
    /// Gets the roles.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole"/> </returns>
    private IEnumerable<IdentityRole> GetRoles()
    {
      return new List<IdentityRole>
      {
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role for tests purposes" },
        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Role 2 for tests purposes"}
      };
    }

    #endregion Private Methods
  }
}