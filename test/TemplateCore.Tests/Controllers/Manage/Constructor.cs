//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Manage
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.Logging;
  using Model;
  using Moq;
  using Services;
  using TemplateCore.Controllers;
  using TemplateCore.Services;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="ManageController"/> .
  /// </summary>
  public class Constructor
  {
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
    /// Initializes a new instance of the <see cref="Constructor"/> class.
    /// Initializes variables
    /// </summary>
    public Constructor()
    {
      this.userManager = MockHelper.TestUserManager<ApplicationUser>(null);
      this.signInManager = MockHelper.GetSignInManager(this.userManager);
      this.emailSender = new Mock<IEmailSender>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
    }

    /// <summary>
    /// Tests the constructor of <see cref="ManageController"/> class
    /// Assert the invoke of the constructor returns an instance of ManageController class.
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // action
      var controller = new ManageController(this.userManager, this.signInManager.Object, this.emailSender.Object, this.smsSender.Object, this.loggerFactory.Object);

      // assert
      Assert.IsType(typeof(ManageController), controller);
    }
  }
}
