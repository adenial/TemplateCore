//-----------------------------------------------------------------------
// <copyright file="Delete.cs" company="Without name">
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
  using Xunit;

  /// <summary>
  /// Class test that tests the method Delete of the class <see cref="RoleController"/>.
  /// </summary>
  public class Delete
  {
    /// <summary>
    /// The controller
    /// </summary>
    private RoleController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<RoleController>> localizer = null;

    private Mock<IRoleService> roleService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Delete"/> class.
    /// </summary>
    public Delete()
    {
      this.roleService = new Mock<IRoleService>();

      this.localizer = new Mock<IStringLocalizer<RoleController>>();
    }

    /// <summary>
    /// Tests the method Delete of the class <see cref="RoleController"/>.
    /// Asserts the invoke of the method returns an instance of the class <see cref="NotFoundResult"/>.
    /// </summary>
    [Fact]
    public void DeleteInvalidName()
    {
      // setup
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = this.controller.Delete(string.Empty);

      // assert
      Assert.IsType(typeof(NotFoundResult), result);
    }

    /// <summary>
    /// Tests the method Delete of the class <see cref="RoleController"/>.
    /// Assert the invoke of the method returns an object of <see cref="RedirectToActionResult"/>.
    /// </summary>
    [Fact]
    public void DeleteOk()
    {
      // setup
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = this.controller.Delete("Test") as RedirectToActionResult;

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }
  }
}