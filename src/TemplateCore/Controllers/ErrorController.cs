namespace TemplateCore.Controllers
{
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// Class ErrorController.
  /// </summary>
  public class ErrorController : Controller
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorController"/> class.
    /// </summary>
    public ErrorController()
    {
    }

    /// <summary>
    /// Intercepts the errors.
    /// </summary>
    /// <returns>Error View.</returns>
    [Route("/Error/Error/")]
    public IActionResult Error()
    {
      // at Startup
      // app.UseExceptionHandler("/Home/Error")
      return View("~/Views/Shared/Error.cshtml");
    }

    /// <summary>
    /// Intercepts the errors from 400 to 600.
    /// </summary>
    /// <returns>NotFound Page or Error.</returns>
    public IActionResult Errors()
    {
      var statusCode = HttpContext.Response.StatusCode;

      if (statusCode == 404)
      {
        return View("~/Views/Shared/NotFound.cshtml");
      }
      else
      {
        // othe errors....
        return View("~/Views/Shared/Error.cshtml");
      }
    }
  }
}