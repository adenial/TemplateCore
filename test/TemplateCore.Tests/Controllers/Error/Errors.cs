namespace TemplateCore.Tests.Controllers.Error
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Routing;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Errors of the class <see cref="ErrorController"/> .
  /// </summary>
  public class Errors
  {
    #region Public Methods

    /// <summary>
    /// Tests the method of the class <see cref="ErrorController"/>.
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult"/>.
    /// Asserts the StatusCode is 404
    /// </summary>
    [Fact]
    public void ErrorsNotFound()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(
        httpContext,
        new RouteData(),
        new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        );

      var controller = new ErrorController();
      var controllerContext = new ControllerContext(actionContext);
      controllerContext.HttpContext.Response.StatusCode = 404;
      controller.ControllerContext = controllerContext;

      // action
      var result = controller.Errors() as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    /// <summary>
    /// Tests the method Errors of the class <see cref="ErrorController"/>.
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult"/>
    /// </summary>
    [Fact]
    public void ErrorsOtherErrors()
    {
      // setup
      var httpContext = new DefaultHttpContext();
      var actionContext = new ActionContext(
        httpContext,
        new RouteData(),
        new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        );

      var controller = new ErrorController();
      var controllerContext = new ControllerContext(actionContext);
      controllerContext.HttpContext.Response.StatusCode = 500;
      controller.ControllerContext = controllerContext;

      // action
      var result = controller.Errors() as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    #endregion Public Methods
  }
}