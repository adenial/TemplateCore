namespace TemplateCore.Tests.Controllers.Home
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Moq;
  using TemplateCore.Controllers;
  using Xunit;

  /// <summary>
  /// Class test that tests the method About of the class <see cref="HomeController"/>.
  /// </summary>
  public class About
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

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="About"/> class.
    /// </summary>
    public About()
    {
      this.localizer = new Mock<IStringLocalizer<HomeController>>();
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method Index of the class <see cref="HomeController"/>
    /// Asserts the invoke of the method returns an instance of the class <see cref="ViewResult"/>
    /// </summary>
    [Fact]
    public void AboutOk()
    {
      // setup
      this.controller = new HomeController(this.localizer.Object);

      // action
      var result = this.controller.About();

      // assert
      Assert.IsType(typeof(ViewResult), result);
    }

    #endregion Public Methods
  }
}