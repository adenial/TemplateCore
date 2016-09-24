//-----------------------------------------------------------------------
// <copyright file="AccessDenied.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Account
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Moq;
  using TemplateCore.Controllers;
  using TemplateCore.Model;
  using TemplateCore.Services;
  using Xunit;

  /// <summary>
  /// Class test that tests the method AccessDenied of the class <see cref="AccountController"/> .
  /// </summary>
  public class AccessDenied
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
    /// Initializes a new instance of the <see cref="AccessDenied"/> class.
    /// Initializes variables
    /// </summary>
    public AccessDenied()
    {
      this.emailSender = new Mock<IEmailSender>();
      this.logger = new Mock<ILogger>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();

      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);

      this.signInManager = MockHelper.GetSignInManager(this.userManager);
    }

    /// <summary>
    /// Tests the method AccessDenied of the class <see cref="AccountController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="ViewResult"/>
    /// </summary>
    [Fact]
    public void AccessDeniedOk()
    {
      // setup
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // action
      var result = this.controller.AccessDenied() as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }
  }
}