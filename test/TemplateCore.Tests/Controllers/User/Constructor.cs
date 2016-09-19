
namespace TemplateCore.Tests.Controllers.User
{
  using Microsoft.Extensions.Localization;
  using Microsoft.Extensions.Logging;
  using Moq;
  using Service.Interfaces;
  using System;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="UserController"/> 
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// The user service
    /// </summary>
    private Mock<IUserService> userService = null;

    /// <summary>
    /// Test the constructor of the class <see cref="UserController"/>. 
    /// Assert the invoke of the method returns an instance of the Controller.
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      this.userService = new Mock<IUserService>();
      Mock<IStringLocalizer<UserController>> localizer = new Mock<IStringLocalizer<UserController>>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();

      // action
      var result = new UserController(this.userService.Object, localizer.Object, logger.Object);

      // assert
      Assert.IsType(typeof(UserController), result);
    }

    /// <summary>
    /// Test the constructor of the class <see cref="UserController"/>.
    /// Assert the invoke of the constructor throws an exception due parameter userService.
    /// </summary>
    [Fact]
    public void ConstructorThrowsExceptionDueParameterUserService()
    {
      // setup.
      UserController controller = null;
      Mock<IStringLocalizer<UserController>> localizer = new Mock<IStringLocalizer<UserController>>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
      Action constructor = () => controller = new UserController(null, localizer.Object, logger.Object);

      // action & assert
      Assert.Throws(typeof(ArgumentNullException), constructor);
    }

    /// <summary>
    /// Test the constructor of the class <see cref="UserController"/>.
    /// Assert the invoke of the constructor throws an exception due parameter localizer.
    /// </summary>
    [Fact]
    public void ConstructorThrowsExceptionDueParameterLocalizer()
    {
      // setup.
      UserController controller = null;
      Mock<IUserService> userService = new Mock<IUserService>();
      Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
      Action constructor = () => controller = new UserController(userService.Object, null, logger.Object);

      // action & assert
      Assert.Throws(typeof(ArgumentNullException), constructor);
    }
  }
}
