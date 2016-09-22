using Microsoft.Extensions.Localization;
using Moq;
using System;
using TemplateCore.Controllers;
using Xunit;

namespace TemplateCore.Tests.Controllers.Home
{
  /// <summary>
  /// Class test that tests the constructor of the class <see cref="HomeController"/>
  /// </summary>
  public class Constructor
  {
    #region Private Fields

    /// <summary>
    /// The controller
    /// </summary>
    private HomeController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<HomeController>> localizer = null;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Tests the constructor of the class <see cref="HomeController"/>.
    /// Assert the invoke of the constructor returns an intance of the class
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      this.localizer = new Mock<IStringLocalizer<HomeController>>();

      // action
      this.controller = new HomeController(this.localizer.Object);

      // assert
      Assert.IsType(typeof(HomeController), this.controller);
    }

    /// <summary>
    /// Tests the constructor of the class <see cref="HomeController"/>.
    /// Assert the invoke of the constructor throws an exception due parameter localizer
    /// </summary>
    [Fact]
    public void ConstructorThrowsExceptionDueParameter()
    {
      // setup, act and assert
      Assert.Throws<ArgumentNullException>(() => this.controller = new HomeController(null));
    }

    #endregion Public Methods
  }
}