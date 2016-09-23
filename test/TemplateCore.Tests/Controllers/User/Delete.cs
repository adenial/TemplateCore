//-----------------------------------------------------------------------
// <copyright file="Delete.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.User
{
  using System;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Delete of the class <see cref="UserController"/>.
  /// </summary>
  public class Delete
  {
    /// <summary>
    /// The controller
    /// </summary>
    private UserController controller = null;

    /// <summary>
    /// The user service
    /// </summary>
    private Mock<IUserService> userService = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<UserController>> localizer = null;

    /// <summary>
    /// Tests the method Delete of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns <see cref="NotFoundResult"/>
    /// </summary>
    [Fact]
    public void DeleteThrowsExceptionDueParameter()
    {
      // setup
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Delete(string.Empty) as NotFoundResult;

      // assert
      Assert.IsType(typeof(NotFoundResult), result);
    }

    /// <summary>
    /// Tests the method Delete of the class <see cref="UserController"/>
    /// Assert the invoke of the method returns <see cref="NotFoundResult"/>
    /// Test case when the user to delete is not found with the provided id.
    /// </summary>
    [Fact]
    public void DeleteUserNotExist()
    {
      // setup
      string id = Guid.NewGuid().ToString();
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.userService.Setup(x => x.Exist(id)).Returns(false);
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Delete(id) as NotFoundResult;

      // assert
      Assert.IsType(typeof(NotFoundResult), result);
    }

    /// <summary>
    /// Tests the method Delete of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="RedirectToActionResult"/>
    /// </summary>
    [Fact]
    public void DeleteOk()
    {
      // setup
      string id = Guid.NewGuid().ToString();
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.userService.Setup(x => x.Exist(id)).Returns(true);
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = this.controller.Delete(id) as RedirectToActionResult;

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }
  }
}
