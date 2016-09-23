//-----------------------------------------------------------------------
// <copyright file="Index.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.User
{
  using System;
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Model;
  using TemplateCore.Service.Interfaces;
  using ViewModels.User;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Index of the class <see cref="UserController"/>.
  /// </summary>
  public class Index
  {
    /// <summary>
    /// The controller
    /// </summary>
    private UserController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<UserController>> localizer = null;

    /// <summary>
    /// The users
    /// </summary>
    private List<AspNetUser> users = null;

    /// <summary>
    /// The user service
    /// </summary>
    private Mock<IUserService> userService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    public Index()
    {
      this.users = new List<AspNetUser>
      {
        new AspNetUser { Email = "test@test.com", Id = Guid.NewGuid(), Name = "Test", Roles = string.Empty, UserName = "test" },
        new AspNetUser { Email = "test1@test.com", Id = Guid.NewGuid(), Name = "Test", Roles = "User", UserName = "test1" }
      };
    }

    /// <summary>
    /// Tests the method Index of the class <see cref="UserController"/>
    /// Assert the invoke of the method returns a List of the type <see cref="UserIndexViewModel"/>
    /// </summary>
    [Fact]
    public void IndexOk()
    {
      // setup
      this.userService = new Mock<IUserService>();
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      this.userService.Setup(x => x.GetAll()).Returns(this.users);
      this.controller = new UserController(this.userService.Object, this.localizer.Object);

      // action
      var result = (this.controller.Index() as ViewResult).Model as List<UserIndexViewModel>;

      // assert
      Assert.IsType(typeof(List<UserIndexViewModel>), result);
    }
  }
}