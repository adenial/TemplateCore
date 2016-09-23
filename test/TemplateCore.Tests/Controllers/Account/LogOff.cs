//-----------------------------------------------------------------------
// <copyright file="LogOff.cs" company="Without name">
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
  using Xunit;

  /// <summary>
  /// Class test that tests the method LogOff of the class <see cref="AccountController"/>
  /// </summary>
  public class LogOff
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
    /// Initializes a new instance of the <see cref="LogOff"/> class.
    /// Initializes variables.
    /// </summary>
    public LogOff()
    {
      this.emailSender = new Mock<IEmailSender>();
      this.logger = new Mock<ILogger>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);
      this.signInManager = MockHelper.GetSignInManager(this.userManager);
    }

    /// <summary>
    /// Tests the method LogOff of the class <see cref="AccountController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="RedirectToActionResult"/>
    /// </summary>
    /// <returns>LogOff Post action.</returns>
    [Fact]
    public async Task LogOffOk()
    {
      // setup
      this.controller = new AccountController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // action
      var result = await this.controller.LogOff();

      // assert
      Assert.IsType(typeof(RedirectToActionResult), result);
    }
  }
}
