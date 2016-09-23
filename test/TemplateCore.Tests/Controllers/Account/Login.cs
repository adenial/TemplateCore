//-----------------------------------------------------------------------
// <copyright file="Login.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Account
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Model;
  using TemplateCore.Services;
  using ViewModels.Account;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Login of the <see cref="AccountController"/> class
  /// </summary>
  public class Login
  {
    /// <summary>
    /// The controller
    /// </summary>
    private AccountController controller = null;

    /// <summary>
    /// The email sender
    /// </summary>
    private Mock<IEmailSender> emailSender = null;

    /// <summary>
    /// The logger
    /// </summary>
    private Mock<ILogger> logger = null;

    /// <summary>
    /// The logger factory
    /// </summary>
    private Mock<ILoggerFactory> loggerFactory = null;

    /// <summary>
    /// The sign in manager
    /// </summary>
    private Mock<SignInManager<ApplicationUser>> signInManager = null;

    /// <summary>
    /// The SMS sender
    /// </summary>
    private Mock<ISmsSender> smsSender = null;

    /// <summary>
    /// The user manager
    /// </summary>
    private UserManager<ApplicationUser> userManager = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Login" /> class.
    /// </summary>
    public Login()
    {
      this.emailSender = new Mock<IEmailSender>();
      this.logger = new Mock<ILogger>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);
      this.signInManager = MockHelper.GetSignInManager(this.userManager);
    }

    /// <summary>
    /// Tests the method Login of the <see cref="AccountController"/> class.
    /// </summary>
    [Fact]
    public void LoginOk()
    {
      // setup
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // action
      var result = this.controller.Login(string.Empty);

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method Login POST action of the <see cref="AccountController" /> class.
    /// Asserts the invoke of the method returns an instance of the class <see cref="RedirectResult" />.
    /// </summary>
    /// <returns>Log in Post.</returns>
    [Fact]
    public async Task LoginPostRedirectResultOk()
    {
      // setup
      var loginModel = new LoginViewModel { UserName = "Test", Password = "1122334455", RememberMe = false };
      string returnUrl = "/User/";
      this.signInManager.Setup(x => x.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
      var urlHelperMock = new Mock<IUrlHelper>();
      urlHelperMock.Setup(x => x.IsLocalUrl(returnUrl)).Returns(true);
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.Url = urlHelperMock.Object;

      // action
      var result = await this.controller.Login(loginModel, returnUrl) as RedirectResult;

      // assert
      Assert.IsType(typeof(RedirectResult), result);
    }

    /// <summary>
    /// Tests the method Login POST action of the <see cref="AccountController" /> class.
    /// Asserts the invoke of the method returns an instance of the class <see cref="RedirectToActionResult" />.
    /// </summary>
    /// <returns>Log in Post.</returns>
    [Fact]
    public async Task LoginPostRedirectToActionResultOk()
    {
      // setup
      var loginModel = new LoginViewModel { UserName = "Test", Password = "1122334455", RememberMe = false };
      string returnUrl = "/User/";
      this.signInManager.Setup(x => x.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
      var urlHelperMock = new Mock<IUrlHelper>();
      urlHelperMock.Setup(x => x.IsLocalUrl(returnUrl)).Returns(false);
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.Url = urlHelperMock.Object;

      // action
      var result = await this.controller.Login(loginModel, returnUrl) as RedirectToActionResult;

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }

    /// <summary>
    /// Tests the method Login POST action of the <see cref="AccountController" /> class.
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult" />.
    /// Test case when the model has errors.
    /// </summary>
    /// <returns>Log in Post.</returns>
    [Fact]
    public async Task LoginPostInvalidModelState()
    {
      // setup
      var loginModel = new LoginViewModel { UserName = "Test", Password = "1122334455", RememberMe = false };
      string returnUrl = "/User/";
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.ModelState.AddModelError(string.Empty, "boom");

      // action
      var result = await this.controller.Login(loginModel, returnUrl) as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method Log in POST action of the class <see cref="AccountController" />.
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult" />.
    /// Test case when the account is locked out.
    /// </summary>
    /// <returns>Log in Post.</returns>
    [Fact]
    public async Task LoginPostLockedAccount()
    {
      // setup
      var loginModel = new LoginViewModel { UserName = "Test", Password = "1122334455", RememberMe = false };
      string returnUrl = "/User/";
      this.signInManager.Setup(x => x.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);
      var urlHelperMock = new Mock<IUrlHelper>();
      urlHelperMock.Setup(x => x.IsLocalUrl(returnUrl)).Returns(false);
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.Url = urlHelperMock.Object;

      // action
      var result = await this.controller.Login(loginModel, returnUrl) as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method Log in POST action of the class <see cref="AccountController"/>.
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult"/>.
    /// Test case when something goes wrong and re display form.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task LoginPostGeneralError()
    {
      // setup
      var loginModel = new LoginViewModel { UserName = "Test", Password = "1122334455", RememberMe = false };
      string returnUrl = "/User/";
      this.signInManager.Setup(x => x.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
      var urlHelperMock = new Mock<IUrlHelper>();
      urlHelperMock.Setup(x => x.IsLocalUrl(returnUrl)).Returns(false);
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);
      this.controller.Url = urlHelperMock.Object;

      // action
      var result = await this.controller.Login(loginModel, returnUrl) as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }
  }
}
