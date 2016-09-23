//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Controllers
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using TemplateCore.Model;
  using TemplateCore.Services;
  using TemplateCore.ViewModels.Account;

  /// <summary>
  /// Class AccountController.
  /// </summary>
  [Authorize]
  public class AccountController : Controller
  {
    private readonly IEmailSender emailSender;
    private readonly ILogger logger;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ISmsSender smsSender;
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="signInManager">The sign in manager.</param>
    /// <param name="emailSender">The email sender.</param>
    /// <param name="smsSender">The SMS sender.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ILoggerFactory loggerFactory)
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.emailSender = emailSender;
      this.smsSender = smsSender;
      this.logger = loggerFactory.CreateLogger<AccountController>();
    }

    /// <summary>
    /// Accesses the denied.
    /// </summary>
    /// <returns>Unauthorized custom view.</returns>
    public IActionResult AccessDenied()
    {
      return this.View();
    }

    /*[HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return View("Error");
      }
      var user = await userManager.FindByIdAsync(userId);
      if (user == null)
      {
        return View("Error");
      }
      var result = await userManager.ConfirmEmailAsync(user, code);
      return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }*/

    /*[HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
      // Request a redirect to the external login provider.
      var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
      var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return Challenge(properties, provider);
    }*/

    /*/// <summary>
    /// Externals the login callback.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <param name="remoteError">The remote error.</param>
    /// <returns>Task&lt;IActionResult&gt;.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
      if (remoteError != null)
      {
        ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        return View(nameof(Login));
      }
      var info = await signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }

      // Sign in the user with this external login provider if the user already has a login.
      var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
      if (result.Succeeded)
      {
        logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
        return RedirectToLocal(returnUrl);
      }
      if (result.RequiresTwoFactor)
      {
        return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
      }
      if (result.IsLockedOut)
      {
        return View("Lockout");
      }
      else
      {
        // If the user does not have an account, then ask the user to create an account.
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["LoginProvider"] = info.LoginProvider;
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
      }
    }*/

    /*/// <summary>
    /// Externals the login confirmation.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>Task&lt;IActionResult&gt;.</returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
    {
      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
          return View("ExternalLoginFailure");
        }
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await userManager.AddLoginAsync(user, info);
          if (result.Succeeded)
          {
            await signInManager.SignInAsync(user, isPersistent: false);
            logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
          }
        }
        AddErrors(result);
      }

      ViewData["ReturnUrl"] = returnUrl;
      return View(model);
    }*/

    /* [HttpGet]
     [AllowAnonymous]
     public IActionResult ForgotPassword()
     {
       return View();
     }*/

    /*[HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await userManager.FindByNameAsync(model.Username);
        if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        {
          // Don't reveal that the user does not exist or is not confirmed
          return View("ForgotPasswordConfirmation");
        }

        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        // Send an email with this link
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        await emailSender.SendEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        return View("ForgotPasswordConfirmation");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }*/

    /*[HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
      return View();
    }*/

    /// <summary>
    /// Logis the specified return URL.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>IActionResult.</returns>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
      this.ViewData["ReturnUrl"] = returnUrl;
      return this.View();
    }

    /// <summary>
    /// Log in with the provided credentials.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>Redirects to the previous view.</returns>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
      this.ViewData["ReturnUrl"] = returnUrl;
      if (this.ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          this.logger.LogInformation(1, "User logged in.");
          return this.RedirectToLocal(returnUrl);
        }

        /* if (result.RequiresTwoFactor)
         {
           return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
         }*/

        if (result.IsLockedOut)
        {
          this.logger.LogWarning(2, "User account locked out.");
          return this.View("Lockout");
        }
        else
        {
          this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return this.View(model);
        }
      }

      // If we got this far, something failed, redisplay form
      return this.View(model);
    }

    /// <summary>
    /// Logs the off.
    /// </summary>
    /// <returns>Redirects to Main page.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
      await this.signInManager.SignOutAsync();
      this.logger.LogInformation(4, "User logged out.");
      return this.RedirectToAction(nameof(HomeController.Index), "Home");
    }

    /*[HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }*/

    /*[HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      if (ModelState.IsValid)
      {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
          // Send an email with this link
          //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
          //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
          //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
          await signInManager.SignInAsync(user, isPersistent: false);
          logger.LogInformation(3, "User created a new account with password.");
          return RedirectToLocal(returnUrl);
        }
        AddErrors(result);
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }*/

    /*[HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
      return code == null ? View("Error") : View();
    }*/

    /*[HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var user = await userManager.FindByNameAsync(model.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
      }

      var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
      }

      AddErrors(result);
      return View();
    }*/

    /*[HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
      return View();
    }*/

    /* [HttpGet]
     [AllowAnonymous]
     public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
     {
       var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
       if (user == null)
       {
         return View("Error");
       }
       var userFactors = await userManager.GetValidTwoFactorProvidersAsync(user);
       var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
       return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
     }*/

    /*[HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendCode(SendCodeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
      if (user == null)
      {
        return View("Error");
      }

      // Generate the token and send it
      var code = await userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
      if (string.IsNullOrWhiteSpace(code))
      {
        return View("Error");
      }

      var message = "Your security code is: " + code;
      if (model.SelectedProvider == "Email")
      {
        await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), "Security Code", message);
      }
      else if (model.SelectedProvider == "Phone")
      {
        await smsSender.SendSmsAsync(await userManager.GetPhoneNumberAsync(user), message);
      }

      return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
    }*/

    /*[HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
    {
      // Require that the user has already logged in via username/password or external login
      var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    }*/

    /* [HttpPost]
     [AllowAnonymous]
     [ValidateAntiForgeryToken]
     public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
     {
       if (!ModelState.IsValid)
       {
         return View(model);
       }

       // The following code protects for brute force attacks against the two factor codes.
       // If a user enters incorrect codes for a specified amount of time then the user account
       // will be locked out for a specified amount of time.
       var result = await signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
       if (result.Succeeded)
       {
         return RedirectToLocal(model.ReturnUrl);
       }
       if (result.IsLockedOut)
       {
         logger.LogWarning(7, "User account locked out.");
         return View("Lockout");
       }
       else
       {
         ModelState.AddModelError(string.Empty, "Invalid code.");
         return View(model);
       }
     }*/

    /// <summary>
    /// Adds the errors.
    /// </summary>
    /// <param name="result">The result.</param>
    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    /// <summary>
    /// Gets the current user asynchronous.
    /// </summary>
    /// <returns>Current user.</returns>
    private Task<ApplicationUser> GetCurrentUserAsync()
    {
      return this.userManager.GetUserAsync(this.HttpContext.User);
    }

    /// <summary>
    /// Redirects to local.
    /// </summary>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>Redirects to the returnUrl.</returns>
    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (this.Url.IsLocalUrl(returnUrl))
      {
        return this.Redirect(returnUrl);
      }
      else
      {
        return this.RedirectToAction(nameof(HomeController.Index), "Home");
      }
    }
  }
}