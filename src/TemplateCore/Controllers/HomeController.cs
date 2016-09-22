namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using System;

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
      return View();
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
      Response.Cookies.Append(
          CookieRequestCultureProvider.DefaultCookieName,
          CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
          new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
      );

      return LocalRedirect(returnUrl);
    }

    /// <summary>
    /// About Action
    /// </summary>
    /// <returns>About Page.</returns>
    public IActionResult About()
    {
      ViewData["Message"] = this.localizer["Your application description page."];

      return View();
    }

    /// <summary>
    /// Contact Action
    /// </summary>
    /// <returns>Contact Page.</returns>
    public IActionResult Contact()
    {
      ViewData["Message"] = this.localizer["Your contact page."];

      return View();
    }
  }
}
