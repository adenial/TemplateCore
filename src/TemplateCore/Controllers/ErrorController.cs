namespace TemplateCore.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
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
    /// Errors this instance.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [Route("/Error/Error/")]
    public IActionResult Error()
    {
      // at Startup
      // app.UseExceptionHandler("/Home/Error")
      return View("~/Views/Shared/Error.cshtml");
    }

    /// <summary>
    /// Errorses this instance.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult Errors()
    {
      var statusCode = HttpContext.Response.StatusCode;

      if (statusCode == 404)
      {
        return View("~/Views/Shared/NotFound.cshtml");
      }
      else
      {
        return View("~/Views/Shared/Error.cshtml");
      }
    }
  }
}