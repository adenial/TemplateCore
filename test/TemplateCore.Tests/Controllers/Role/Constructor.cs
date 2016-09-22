/// <summary>
/// The Role namespace.
/// </summary>
namespace TemplateCore.Tests.Controllers.Role
{
  using Microsoft.Extensions.Localization;
  using Moq;
  using System;
  using TemplateCore.Controllers;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="RoleController"/> .
  /// </summary>
  public class Constructor
  {
    #region Private Fields

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<RoleController>> localizer = null;

    /// <summary>
    /// The role service
    /// </summary>
    private Mock<IRoleService> roleService = null;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Tests the constructor of the class <see cref="RoleController"/>
    /// Assert the constructor returns an instance of the class <see cref="RoleController"/>.
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      this.roleService = new Mock<IRoleService>();
      this.localizer = new Mock<IStringLocalizer<RoleController>>();

      // action
      var controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // assert
      Assert.IsType(typeof(RoleController), controller);
    }

    /// <summary>
    /// Tests the constructor of the class <see cref="RoleController"/>.
    /// Asserts the invoke of the constructor throws an exception of type <see cref="ArgumentNullException"/>.
    /// Throws exception due parameter localizer
    /// </summary>
    [Fact]
    public void ConstructorThrowsExceptionDueParameterLocalizer()
    {
      // setup
      this.roleService = new Mock<IRoleService>();

      Assert.Throws<ArgumentNullException>(() => new RoleController(this.roleService.Object, null));
    }

    /// <summary>
    /// Tests the constructor of the class <see cref="RoleController"/>.
    /// Asserts the invoke of the constructor throws an exception of type <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void ConstructorThrowsExceptionDueParameterService()
    {
      // setup act and assert
      this.localizer = new Mock<IStringLocalizer<RoleController>>();

      Assert.Throws<ArgumentNullException>(() => new RoleController(null, this.localizer.Object));
    }

    #endregion Public Methods
  }
}