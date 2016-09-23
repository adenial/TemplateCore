//-----------------------------------------------------------------------
// <copyright file="Create.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Role
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Service.Interfaces;
  using ViewModels.Role;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Create of the class <see cref="RoleController" />.
  /// </summary>
  public class Create
  {
    /// <summary>
    /// The controller
    /// </summary>
    private RoleController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<RoleController>> localizer = null;

    /// <summary>
    /// The role service
    /// </summary>
    private Mock<IRoleService> roleService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Create"/> class.
    /// </summary>
    public Create()
    {
      this.roleService = new Mock<IRoleService>();
      this.localizer = new Mock<IStringLocalizer<RoleController>>();
    }

    /// <summary>
    /// Tests the method create of the class <see cref="RoleController"/>
    /// Assert the invoke of the method returns an instance of the  class <see cref="AdminRoleCreateViewModel"/>.
    /// </summary>
    [Fact]
    public void CreateOk()
    {
      // setup
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = (this.controller.Create() as ViewResult).Model as AdminRoleCreateViewModel;

      // assert
      Assert.IsType(typeof(AdminRoleCreateViewModel), result);
    }

    /// <summary>
    /// Tests the method Create POST action of the class <see cref="RoleController"/>.
    /// Assert the invoke of the method returns a instance of the class <see cref="AdminRoleCreateViewModel"/>.
    /// </summary>
    [Fact]
    public void CreatePostInvalidModelState()
    {
      // setup
      var model = new AdminRoleCreateViewModel { Name = "Test" };
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);
      this.controller.ModelState.AddModelError(string.Empty, "There's already a role with the provided name.");

      // action
      var result = (this.controller.Create(model) as ViewResult).Model as AdminRoleCreateViewModel;

      // assert
      Assert.IsType(typeof(AdminRoleCreateViewModel), result);
    }

    /// <summary>
    /// Tests the method Create POST action of the class <see cref="RoleController"/>.
    /// Asserts the invoke of the method returns an instance of the class <see cref="AdminRoleCreateViewModel"/>.
    /// Tests case when the name of the new role to insert, already exists.
    /// </summary>
    [Fact]
    public void CreatePostInvalidName()
    {
      // setup
      var localizedString = new LocalizedString("There's already a role with the provided name.", "There's already a role with the provided name.");
      var model = new AdminRoleCreateViewModel { Name = "Test" };
      this.roleService.Setup(x => x.CanInsert(model.Name)).Returns(false);
      this.localizer.Setup(x => x["There's already a role with the provided name."]).Returns(localizedString);
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = (this.controller.Create(model) as ViewResult).Model as AdminRoleCreateViewModel;

      // assert
      Assert.IsType(typeof(AdminRoleCreateViewModel), result);
      Assert.False(this.controller.ModelState.IsValid);
    }

    /// <summary>
    /// Tests the method Create POST action of the class <see cref="RoleController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="RedirectToActionResult"/>.
    /// </summary>
    [Fact]
    public void CreatePostOk()
    {
      // setup
      var model = new AdminRoleCreateViewModel { Name = "Test" };
      this.roleService.Setup(x => x.CanInsert(model.Name)).Returns(true);
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = this.controller.Create(model);

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }
  }
}