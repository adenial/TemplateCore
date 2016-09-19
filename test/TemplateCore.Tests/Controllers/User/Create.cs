namespace TemplateCore.Tests.Controllers.User
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Microsoft.Extensions.Logging;
  using Moq;
  using Service.Interfaces;
  using System;
  using System.Collections.Generic;
  using TemplateCore.Controllers;
  using TemplateCore.ViewModels.User;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Create of the class <see cref="UserController"/> .
  /// </summary>
  public class Create
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
    /// Test the method Create of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="UserCreateViewModel"/>. 
    /// </summary>
    [Fact]
    public void CreateGetAction()
    {
      // setup
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      List<IdentityRole> roles = new List<IdentityRole>();
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 1" });
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 2" });
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 3" });
      Mock<IUserService> userService = new Mock<IUserService>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
      userService.Setup(x => x.GetAllRoles()).Returns(roles);

      this.controller = new UserController(userService.Object, this.localizer.Object, logger.Object);

      // action
      var result = (this.controller.Create() as ViewResult).Model as UserCreateViewModel;

      // assert
      Assert.IsType(typeof(UserCreateViewModel), result);
    }

    /// <summary>
    /// Test the method Create POST Action of the class <see cref="UserController"/>.
    /// Assert the invoke of the method redirects to Index.
    /// Happy path.
    /// </summary>
    [Fact]
    public void CreatePostOk()
    {
      // setup
      this.localizer = new Mock<IStringLocalizer<UserController>>();
      var model = this.CreateViewModel();
      Mock<IUserService> userService = new Mock<IUserService>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
      userService.Setup(x => x.CanInsertUserName(model.UserName)).Returns(true);
      userService.Setup(x => x.CanInsertEmail(model.Email)).Returns(true);

      this.controller = new UserController(userService.Object, this.localizer.Object, logger.Object);

      // action
      var result = this.controller.Create(model) as RedirectToActionResult;

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }

    /// <summary>
    /// Test the method Create POST Action of the class <see cref="UserController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="UserCreateViewModel"/>.
    /// Assert the ModelState error is due the field username.
    /// </summary>
    [Fact]
    public void CreatePostInvalidUsername()
    {
      // setup
      var model = this.CreateViewModel();

      this.localizer = new Mock<IStringLocalizer<UserController>>();

      // TODO: fix this unit test.
      // NOT WORKING.
      this.localizer.Setup(x => x.GetString(It.IsAny<string>())).Returns(new LocalizedString("", "There's already a user with the provided email."));


      //var localizedString = new LocalizedString("test", "test");

      //this.localizer.Setup(x => x.GetString(It.IsAny<string>())).Returns(localizedString);
      // this.localizer.SetupGet(x => x.GetString("There's already a user with the provided username.").Name).Returns("There's already a user with the provided username.");

      //var localizedString = new LocalizedString("There's already a user with the provided username.", "There's already a user with the provided username.");

      //this.localizer.Setup(x => x.GetString("There's already a user with the provided username.").Name).Returns("There's already a user with the provided username.");
      //this.localizer.SetupProperty(x => x.GetString("There's already a user with the provided username.").Name).SetReturnsDefault("There's already a user with the provided username.");

      Mock<IUserService> userService = new Mock<IUserService>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
      userService.Setup(x => x.CanInsertUserName(model.UserName)).Returns(false);
      userService.Setup(x => x.GetAllRoles()).Returns(this.CreateRoles());
      this.controller = new UserController(userService.Object, this.localizer.Object, logger.Object);

      // action
      var result = (this.controller.Create(model) as ViewResult).Model as UserCreateViewModel;

      // assert.
      Assert.IsType(typeof(UserCreateViewModel), result);
    }

    /// <summary>
    /// Creates the roles.
    /// </summary>
    /// <returns>IEnumerable&lt;IdentityRole&gt;.</returns>
    public IEnumerable<IdentityRole> CreateRoles()
    {
      List<IdentityRole> roles = new List<IdentityRole>();
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 1" });
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 2" });
      roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Test 3" });

      return roles;
    }

    /// <summary>
    /// Creates the view model.
    /// </summary>
    /// <returns>UserCreateViewModel.</returns>
    private UserCreateViewModel CreateViewModel()
    {
      UserCreateViewModel model = new UserCreateViewModel();
      model.Email = "test@test.com";
      model.Name = "Test for testing purposes.";
      model.UserName = "Tester";
      model.Roles = new List<UserRoleCreateViewModel>
      {
        new UserRoleCreateViewModel { Check = false, Id = Guid.NewGuid().ToString(), Name = "Admin" },
        new UserRoleCreateViewModel { Check = true, Id = Guid.NewGuid().ToString(), Name = "Test" }
      };

      return model;
    }
  }
}
