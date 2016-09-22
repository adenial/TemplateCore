namespace TemplateCore.Tests.Controllers.Account
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using Model;
  using Moq;
  using Services;
  using System;
  using System.Collections.Generic;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="AccountController"/> .
  /// </summary>
  public class Constructor
  {
    #region Private Fields

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

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Constructor"/> class.
    /// Initializes variables.
    /// </summary>
    public Constructor()
    {
      this.emailSender = new Mock<IEmailSender>();
      this.logger = new Mock<ILogger>();
      this.smsSender = new Mock<ISmsSender>();
      this.loggerFactory = new Mock<ILoggerFactory>();
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// https://github.com/aspnet/Identity/blob/master/test/Shared/MockHelpers.cs
    /// </summary>
    public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
    {
      store = store ?? new Mock<IUserStore<TUser>>().Object;
      var options = new Mock<IOptions<IdentityOptions>>();
      var idOptions = new IdentityOptions();
      idOptions.Lockout.AllowedForNewUsers = false;
      options.Setup(o => o.Value).Returns(idOptions);
      var userValidators = new List<IUserValidator<TUser>>();
      var validator = new Mock<IUserValidator<TUser>>();
      userValidators.Add(validator.Object);
      var pwdValidators = new List<PasswordValidator<TUser>>();
      pwdValidators.Add(new PasswordValidator<TUser>());
      var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
          userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
          new IdentityErrorDescriber(), null,
          new Mock<ILogger<UserManager<TUser>>>().Object);
      validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
          .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
      return userManager;
    }

    /// <summary>
    /// Tests the constructor of the class <see cref="AccountController"/>
    /// Asserts the invoke of the constructor returns an instance of the class <see cref="AccountController"/>.
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      var userStore = new Mock<IUserStore<ApplicationUser>>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      var claimsManager = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
      var options = new Mock<IOptions<IdentityOptions>>();
      var user = new ApplicationUser { Name = "Test" };
      var identityOptions = new IdentityOptions { SecurityStampValidationInterval = TimeSpan.Zero };
      var id = new ClaimsIdentity(identityOptions.Cookies.ApplicationCookieAuthenticationScheme);
      id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
      var principal = new ClaimsPrincipal(id);
      this.userManager = TestUserManager<ApplicationUser>(null);

      // https://github.com/aspnet/Identity/blob/4ef80fabf66624b464e77ac9dd6e8c4461759e0c/test/Microsoft.AspNetCore.Identity.Test/SecurityStampValidatorTest.cs
      this.signInManager = new Mock<SignInManager<ApplicationUser>>(userManager,
     contextAccessor.Object, claimsManager.Object, options.Object, null);
      signInManager.Setup(s => s.ValidateSecurityStampAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user).Verifiable();
      signInManager.Setup(s => s.CreateUserPrincipalAsync(user)).ReturnsAsync(principal).Verifiable();

      // action
      this.controller = new AccountController(
        this.userManager,
        this.signInManager.Object,
        this.emailSender.Object,
        this.smsSender.Object,
        this.loggerFactory.Object);

      // assert
      Assert.IsType(typeof(AccountController), this.controller);
    }

    #endregion Public Methods
  }
}