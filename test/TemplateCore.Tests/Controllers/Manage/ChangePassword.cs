//-----------------------------------------------------------------------
// <copyright file="ChangePassword.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Manage
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.Logging;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Model;
  using TemplateCore.Services;
  using ViewModels.Manage;
  using Xunit;

  /// <summary>
  /// Class test that tests the method ChangePassword of the <see cref="ManageController"/> class.
  /// </summary>
  public class ChangePassword
  {
    /// <summary>
    /// The controller
    /// </summary>
    private ManageController controller = null;

    /// <summary>
    /// The user manager
    /// </summary>
    private UserManager<ApplicationUser> userManager = null;

    /// <summary>
    /// The sign in manager
    /// </summary>
    private Mock<SignInManager<ApplicationUser>> signInManager = null;

    /// <summary>
    /// The email sender
    /// </summary>
    private Mock<IEmailSender> emailSender = null;

    /// <summary>
    /// The SMS sender
    /// </summary>
    private Mock<ISmsSender> smsSender = null;

    /// <summary>
    /// The logger factory
    /// </summary>
    private Mock<ILoggerFactory> loggerFactory = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePassword"/> class.
    /// Initializes variables
    /// </summary>
    public ChangePassword()
    {
      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);
      this.signInManager = MockHelper.GetSignInManager(this.userManager);
      this.emailSender = new Mock<IEmailSender>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
    }

    /// <summary>
    /// Tests the method ChangePassword of the <see cref="ManageController"/>class.
    /// Asserts the invoke of the method returns an object of the <see cref="ViewResult"/> class.
    /// </summary>
    [Fact]
    public void ChangePasswordOk()
    {
      // setup
      this.controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // action
      var result = this.controller.ChangePassword() as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method ChangePassword POST action of the <see cref="ManageController"/> class.
    /// Asserts the invoke of the method returns an instance of the <see cref="ChangePasswordViewModel"/> class
    /// </summary>
    /// <returns>View Model.</returns>
    [Fact]
    public async Task ChangePasswordPostInvalidModelState()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
      var controllerContext = new ControllerContext(actionContext);
      var model = new ChangePasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455", OldPassword = "Tests" };
      this.controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ControllerContext = controllerContext;
      this.controller.ModelState.AddModelError(string.Empty, "Invalid password");

      // action
      var result = (await this.controller.ChangePassword(model) as ViewResult).Model as ChangePasswordViewModel;

      // assert
      Assert.IsType(typeof(ChangePasswordViewModel), result);
    }

    /// <summary>
    /// Tests the method ChangePassword POST action of the <see cref="ManageController"/> class.
    /// Asserts the invoke of the method returns an instance of the <see cref="RedirectToActionResult"/> class
    /// </summary>
    /// <returns>RedirectToActionResult to Index.</returns>
    [Fact]
    public async Task ChangePasswordPostErrorOk()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
      var controllerContext = new ControllerContext(actionContext);
      var model = new ChangePasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455", OldPassword = "Tests" };
      this.controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ControllerContext = controllerContext;

      // action
      var result = await this.controller.ChangePassword(model);

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }

    /// <summary>
    /// Tests the method ChangePassword POST action of the <see cref="ManageController" /> class.
    /// Asserts the invoke of the method returns an instance of the <see cref="RedirectToActionResult" /> class
    /// </summary>
    /// <returns>RedirectToActionResult to Index.</returns>
    [Fact]
    public async Task ChangePasswordPostOk()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
      var controllerContext = new ControllerContext(actionContext);
      var model = new ChangePasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455", OldPassword = "Tests" };
      var userManagerMock = MockHelper.MockUserManager<ApplicationUser>();
      var user = new ApplicationUser { UserName = "Test" };
      userManagerMock.Setup(x => x.GetUserAsync(httpContext.User)).ReturnsAsync(user);
      userManagerMock.Setup(x => x.ChangePasswordAsync(user, model.OldPassword, model.NewPassword)).ReturnsAsync(IdentityResult.Success);

      this.controller = new ManageController(userManagerMock.Object, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ControllerContext = controllerContext;

      // action
      var result = await this.controller.ChangePassword(model);

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }

    /// <summary>
    /// Tests the method ChangePassword POST action of the <see cref="ManageController" /> class.
    /// Asserts the invoke of the method returns an instance of the <see cref="ChangePasswordViewModel" /> class.
    /// Test case when there are errors while updating the password.
    /// </summary>
    /// <returns>View Model.</returns>
    [Fact]
    public async Task ChangePasswordPosErrortOk()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
      var controllerContext = new ControllerContext(actionContext);
      var model = new ChangePasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455", OldPassword = "Tests" };
      var userManagerMock = MockHelper.MockUserManager<ApplicationUser>();
      var user = new ApplicationUser { UserName = "Test" };
      userManagerMock.Setup(x => x.GetUserAsync(httpContext.User)).ReturnsAsync(user);

      // var identityResult = new Mock<IdentityResult>(GetErrors());

      // identityResult.SetupGet(x => It.IsAny<IEnumerable<IdentityError>>()).Returns(GetErrors());
      // identityResult.Setup(x => x.Errors).Returns(this.GetErrors());
      var identityResult = IdentityResult.Failed(this.GetErrors().ToArray());

      userManagerMock.Setup(x => x.ChangePasswordAsync(user, model.OldPassword, model.NewPassword)).ReturnsAsync(identityResult);

      this.controller = new ManageController(userManagerMock.Object, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ControllerContext = controllerContext;

      // action
      var result = (await this.controller.ChangePassword(model) as ViewResult).Model as ChangePasswordViewModel;

      // assert
      Assert.IsType(typeof(ChangePasswordViewModel), result);
    }

    /// <summary>
    /// Get errors
    /// </summary>
    /// <returns>List of type <see cref="IdentityError"/>.</returns>
    private IEnumerable<IdentityError> GetErrors()
    {
      return new List<IdentityError>
      {
        new IdentityError { Description = "Error 1", Code = "New Password" },
        new IdentityError { Description = "Error 1", Code = "Old Password" }
      };
    }
  }
}
