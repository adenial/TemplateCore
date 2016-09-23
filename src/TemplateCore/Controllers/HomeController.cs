//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Controllers
{
  using System;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;

  /// <summary>
  /// Class HomeController.
  /// </summary>
  public class HomeController : Controller
  {
    /// <summary>
    /// The localizer
    /// </summary>
    private readonly IStringLocalizer<HomeController> localizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController" /> class.
    /// </summary>
    /// <param name="localizer">The localizer.</param>
    public HomeController(IStringLocalizer<HomeController> localizer)
    {
      if (localizer == null)
      {
        throw new ArgumentNullException("localizer");
      }

      this.localizer = localizer;
    }

    /// <summary>
    /// Index Action
    /// </summary>
    /// <returns>Index Page.</returns>
    public IActionResult Index()
    {
      return this.View();
    }

    /// <summary>
    /// Sets the language.
    /// </summary>
    /// <param name="culture">The culture.</param>
    /// <param name="returnUrl">The return URL.</param>
    /// <returns>Microsoft.AspNetCore.Mvc.IActionResult.</returns>
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
      this.Response.Cookies.Append(
          CookieRequestCultureProvider.DefaultCookieName,
          CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
          new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

      return this.LocalRedirect(returnUrl);
    }

    /// <summary>
    /// About Action
    /// </summary>
    /// <returns>About Page.</returns>
    public IActionResult About()
    {
      this.ViewData["Message"] = this.localizer["Your application description page."];

      return this.View();
    }

    /// <summary>
    /// Contact Action
    /// </summary>
    /// <returns>Contact Page.</returns>
    public IActionResult Contact()
    {
      this.ViewData["Message"] = this.localizer["Your contact page."];

      return this.View();
    }
  }
}
