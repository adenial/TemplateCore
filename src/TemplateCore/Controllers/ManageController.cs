//-----------------------------------------------------------------------
// <copyright file="ManageController.cs" company="Without name">
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
  using TemplateCore.ViewModels.Manage;

  /// <summary>
  /// Class ManageController.
  /// </summary>
  [Authorize]
  public class ManageController : Controller
  {
    private readonly IEmailSender emailSender;
    private readonly ILogger logger;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly ISmsSender smsSender;
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManageController" /> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="signInManager">The sign in manager.</param>
    /// <param name="emailSender">The email sender.</param>
    /// <param name="smsSender">The SMS sender.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public ManageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ISmsSender smsSender, ILoggerFactory loggerFactory)
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.emailSender = emailSender;
      this.smsSender = smsSender;
      this.logger = loggerFactory.CreateLogger<ManageController>();
    }

    /// <summary>
    /// Changes the password.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [HttpGet]
    public IActionResult ChangePassword()
    {
      return this.View();
    }

    /// <summary>
    /// Changes the password.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Task&lt;IActionResult&gt;.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
      if (!this.ModelState.IsValid)
      {
        return this.View(model);
      }

      var user = await this.GetCurrentUserAsync();
      if (user != null)
      {
        var result = await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
          await this.signInManager.SignInAsync(user, isPersistent: false);

          // this.logger.LogInformation(3, "User changed their password successfully.");
          return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.ChangePasswordSuccess });
        }

        this.AddErrors(result);
        return this.View(model);
      }

      return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.Error });
    }

    /// <summary>
    /// Main view page of Account.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>View to manage the account.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(ManageMessageId? message = null)
    {
      this.ViewData["StatusMessage"] =
          message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
          : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
          : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
          : message == ManageMessageId.Error ? "An error has occurred."
          : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
          : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
          : string.Empty;

      var user = await this.GetCurrentUserAsync();
      if (user == null)
      {
        return this.View("Error");
      }

      var model = new IndexViewModel
      {
        HasPassword = await this.userManager.HasPasswordAsync(user),
        PhoneNumber = await this.userManager.GetPhoneNumberAsync(user),
        TwoFactor = await this.userManager.GetTwoFactorEnabledAsync(user),
        Logins = await this.userManager.GetLoginsAsync(user),
        BrowserRemembered = await this.signInManager.IsTwoFactorClientRememberedAsync(user)
      };
      return this.View(model);
    }

    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
    {
      ManageMessageId? message = ManageMessageId.Error;
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        var result = await userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
        if (result.Succeeded)
        {
          await signInManager.SignInAsync(user, isPersistent: false);
          message = ManageMessageId.RemoveLoginSuccess;
        }
      }
      return RedirectToAction(nameof(ManageLogins), new { Message = message });
    }*/

    /*/// <summary>
    /// View to add phone number
    /// </summary>
    /// <returns>View to add phone number</returns>
    public IActionResult AddPhoneNumber()
    {
      return View();
    }*/

    /*/// <summary>
      /// POST action to add phone number
      /// </summary>
      /// <param name="model">The model.</param>
      /// <returns>Redirects to verify phone number.</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
      {
        if (!ModelState.IsValid)
        {
          return View(model);
        }
        // Generate the token and send it
        var user = await GetCurrentUserAsync();
        if (user == null)
        {
          return View("Error");
        }
        var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
        await smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
        return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
      }*/

    /*/// <summary>
    /// Enable two factor Authentication View.
    /// </summary>
    /// <returns>Redirects to Index Page.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        await userManager.SetTwoFactorEnabledAsync(user, true);
        await signInManager.SignInAsync(user, isPersistent: false);
        logger.LogInformation(1, "User enabled two-factor authentication.");
      }
      return RedirectToAction(nameof(Index), "Manage");
    }*/

    /*//
    // POST: /Manage/DisableTwoFactorAuthentication
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DisableTwoFactorAuthentication()
    {
      var user = await GetCurrentUserAsync();
      if (user != null)
      {
        await userManager.SetTwoFactorEnabledAsync(user, false);
        await signInManager.SignInAsync(user, isPersistent: false);
        logger.LogInformation(2, "User disabled two-factor authentication.");
      }
      return RedirectToAction(nameof(Index), "Manage");
    }*/

    /*//
    // GET: /Manage/VerifyPhoneNumber
    [HttpGet]
    public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
    {
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var code = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
      // Send an SMS to verify the phone number
      return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
    }*/

    /* //
     // POST: /Manage/VerifyPhoneNumber
     [HttpPost]
     [ValidateAntiForgeryToken]
     public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
     {
       if (!ModelState.IsValid)
       {
         return View(model);
       }
       var user = await GetCurrentUserAsync();
       if (user != null)
       {
         var result = await userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
         if (result.Succeeded)
         {
           await signInManager.SignInAsync(user, isPersistent: false);
           return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
         }
       }
       // If we got this far, something failed, redisplay the form
       ModelState.AddModelError(string.Empty, "Failed to verify phone number");
       return View(model);
     }*/

    /*/// <summary>
    /// Removes the phone number.
    /// </summary>
    /// <returns>Task&lt;IActionResult&gt;.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePhoneNumber()
    {
      var user = await this.GetCurrentUserAsync();
      if (user != null)
      {
        var result = await this.userManager.SetPhoneNumberAsync(user, null);
        if (result.Succeeded)
        {
          await this.signInManager.SignInAsync(user, isPersistent: false);
          return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.RemovePhoneSuccess });
        }
      }

      return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.Error });
    }*/

    /// <summary>
    /// Sets the password.
    /// </summary>
    /// <returns>View to set password.</returns>
    [HttpGet]
    public IActionResult SetPassword()
    {
      return this.View();
    }

    /// <summary>
    /// Sets the password.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Task&lt;IActionResult&gt;.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
      if (!this.ModelState.IsValid)
      {
        return this.View(model);
      }

      var user = await this.GetCurrentUserAsync();
      if (user != null)
      {
        var result = await this.userManager.AddPasswordAsync(user, model.NewPassword);
        if (result.Succeeded)
        {
          await this.signInManager.SignInAsync(user, isPersistent: false);
          return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.SetPasswordSuccess });
        }

        this.AddErrors(result);
        return this.View(model);
      }

      return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.Error });
    }

    /*//GET: /Manage/ManageLogins
    [HttpGet]
    public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
    {
      ViewData["StatusMessage"] =
          message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
          : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
          : message == ManageMessageId.Error ? "An error has occurred."
          : string.Empty;
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var userLogins = await userManager.GetLoginsAsync(user);
      var otherLogins = signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
      ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
      return View(new ManageLoginsViewModel
      {
        CurrentLogins = userLogins,
        OtherLogins = otherLogins
      });
    }*/

    /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LinkLogin(string provider)
        {
          // Request a redirect to the external login provider to link a login for the current user
          var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
          var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, userManager.GetUserId(User));
          return Challenge(properties, provider);
        }*/

    /*[HttpGet]
    public async Task<ActionResult> LinkLoginCallback()
    {
      var user = await GetCurrentUserAsync();
      if (user == null)
      {
        return View("Error");
      }
      var info = await signInManager.GetExternalLoginInfoAsync(await userManager.GetUserIdAsync(user));
      if (info == null)
      {
        return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
      }
      var result = await userManager.AddLoginAsync(user, info);
      var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
      return RedirectToAction(nameof(ManageLogins), new { Message = message });
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
    /// <returns>Logged User.</returns>
    private Task<ApplicationUser> GetCurrentUserAsync()
    {
      return this.userManager.GetUserAsync(this.HttpContext.User);
    }
  }
}