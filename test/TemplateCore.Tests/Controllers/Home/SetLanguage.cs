//-----------------------------------------------------------------------
// <copyright file="SetLanguage.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Home
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.Localization;
  using Moq;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the method SetLanguage of the class <see cref="HomeController"/>
  /// </summary>
  public class SetLanguage
  {
    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<HomeController>> localizer = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetLanguage"/> class.
    /// </summary>
    public SetLanguage()
    {
      this.localizer = new Mock<IStringLocalizer<HomeController>>();
    }

    /// <summary>
    /// Tests the method SetLanguage POST action of the class <see cref="HomeController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="LocalRedirectResult"/>.
    /// </summary>
    [Fact]
    public void SetLanguageOk()
    {
      // setup
      string culture = "en-US";
      string returnUrl = "Home";

      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(
        httpContext,
        new RouteData(),
        new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());

      var controllerContext = new ControllerContext(actionContext);
      var controller = new HomeController(this.localizer.Object);
      controller.ControllerContext = controllerContext;

      // action
      var result = controller.SetLanguage(culture, returnUrl) as LocalRedirectResult;

      // assert
      Assert.IsType(typeof(LocalRedirectResult), result);
    }
  }
}