//-----------------------------------------------------------------------
// <copyright file="SetPassword.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Manage
{
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
  /// Class test that tests the method SetPassword of <see cref="ManageController"/> class.
  /// </summary>
  public class SetPassword
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
    /// Initializes a new instance of the <see cref="SetPassword"/> class.
    /// Initializes variables.
    /// </summary>
    public SetPassword()
    {
      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);
      this.signInManager = MockHelper.GetSignInManager(this.userManager);
      this.emailSender = new Mock<IEmailSender>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
    }

    /// <summary>
    /// Tests the method SetPassword of <see cref="ManageController"/> class.
    /// Assert the invoke of the method returns an instance of <see cref="ViewResult"/>
    /// </summary>
    [Fact]
    public void SetPasswordOk()
    {
      // setup
      this.controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // act
      var result = this.controller.SetPassword();

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method SetPassword of <see cref="ManageController"/> class.
    /// Asserts the invoke of the method returns an instance of <see cref="SetPasswordViewModel"/> class.
    /// </summary>
    /// <returns>view Model.</returns>
    [Fact]
    public async Task SetPasswordPostInvalidModelState()
    {
      // setup
      var model = new SetPasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455" };
      this.controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ModelState.AddModelError(string.Empty, "boom!");

      // action
      var result = (await this.controller.SetPassword(model) as ViewResult).Model as SetPasswordViewModel;

      // assert
      Assert.IsType(typeof(SetPasswordViewModel), result);
    }

    /// <summary>
    /// Tests the method SetPassword POST action of <see cref="ManageController"/> class
    /// Happy path.
    /// Tests the invoke of the method returns an instance of <see cref="RedirectToActionResult"/> class
    /// </summary>
    /// <returns>Index View.</returns>
    [Fact]
    public async Task SetPasswordPostOk()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
      var controllerContext = new ControllerContext(actionContext);
      var model = new SetPasswordViewModel { ConfirmPassword = "1122334455", NewPassword = "1122334455" };
      var userManagerMock = MockHelper.MockUserManager<ApplicationUser>();
      var user = new ApplicationUser { UserName = "Test" };
      userManagerMock.Setup(x => x.GetUserAsync(httpContext.User)).ReturnsAsync(user);
      userManagerMock.Setup(x => x.AddPasswordAsync(user, model.NewPassword)).ReturnsAsync(IdentityResult.Success);
      this.controller = new ManageController(userManagerMock.Object, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ControllerContext = controllerContext;

      // action
      var result = await this.controller.SetPassword(model);

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }
  }
}
