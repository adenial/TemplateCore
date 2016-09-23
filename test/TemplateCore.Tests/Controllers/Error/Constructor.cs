//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Controllers.Error
{
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the contructor of the class <see cref="ErrorController"/>.
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// Tests the contructor of the class <see cref="ErrorController"/>
    /// Assert the invoke of the constructor returns an instance of the class.
    /// </summary>
    [Fact]
    public void ErrorControllerOk()
    {
      // setup

      // action
      var controller = new ErrorController();

      // assert
      Assert.IsType(typeof(ErrorController), controller);
    }
  }
}