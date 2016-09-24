//-----------------------------------------------------------------------
// <copyright file="MockHelper.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using Moq;
  using TemplateCore.Model;

  /// <summary>
  /// Class MockHelper.
  /// </summary>
  public static class MockHelper
  {
    /// <summary>
    /// Tests the user manager.
    /// https://github.com/aspnet/Identity/blob/master/test/Shared/MockHelpers.cs
    /// </summary>
    /// <typeparam name="TUser">The type of the t user.</typeparam>
    /// <param name="store">The store.</param>
    /// <returns>Object of the type <see cref="UserManager{TUser}"/> .</returns>
    public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null)
      where TUser : class
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
      var userManager = new UserManager<TUser>(
        store,
        options.Object,
        new PasswordHasher<TUser>(),
        userValidators,
        pwdValidators,
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        null,
        new Mock<ILogger<UserManager<TUser>>>().Object);
      validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>())).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
      return userManager;
    }

    /// <summary>
    /// Mocks the user manager.
    /// </summary>
    /// <typeparam name="TUser">The type of the t user.</typeparam>
    /// <returns>Mock&lt;UserManager&lt;TUser&gt;&gt;.</returns>
    public static Mock<UserManager<TUser>> MockUserManager<TUser>()
      where TUser : class
    {
      var store = new Mock<IUserStore<TUser>>();
      var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);

      // mgr.Object.UserValidators.Add(new UserValidator<TUser>());
      // mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
      return mgr;
    }

    /// <summary>
    /// Mocks the SigInManager.
    /// </summary>
    /// <param name="manager">The manager.</param>
    /// <returns>Mock object of the class <see cref="SignInManager{TUser}" /> .</returns>
    public static Mock<SignInManager<ApplicationUser>> GetSignInManager(UserManager<ApplicationUser> manager)
    {
      var userStore = new Mock<IUserStore<ApplicationUser>>();
      var contextAccessor = new Mock<IHttpContextAccessor>();
      var claimsManager = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
      var options = new Mock<IOptions<IdentityOptions>>();
      var user = new ApplicationUser { Name = "Test" };
      var identityOptions = new IdentityOptions { SecurityStampValidationInterval = TimeSpan.Zero };
      var id = new ClaimsIdentity(identityOptions.Cookies.ApplicationCookieAuthenticationScheme);
      id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
      var principal = new ClaimsPrincipal(id);

      var signInManager = new Mock<SignInManager<ApplicationUser>>(manager, contextAccessor.Object, claimsManager.Object, options.Object, null);
      signInManager.Setup(s => s.ValidateSecurityStampAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user).Verifiable();
      signInManager.Setup(s => s.CreateUserPrincipalAsync(user)).ReturnsAsync(principal).Verifiable();

      return signInManager;
    }
  }
}
