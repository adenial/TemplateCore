//-----------------------------------------------------------------------
// <copyright file="Error.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Error
{
  using Microsoft.AspNetCore.Mvc;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Error of the class <see cref="ErrorController"/>.
  /// </summary>
  public class Error
  {
    /// <summary>
    /// Tests the method Error of the class <see cref="ErrorController"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="ViewResult"/>
    /// </summary>
    [Fact]
    public void ErrorOk()
    {
      // setup
      var controller = new ErrorController();

      // action
      var result = controller.Error() as ViewResult;

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }
  }
}