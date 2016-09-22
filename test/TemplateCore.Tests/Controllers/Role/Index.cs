namespace TemplateCore.Tests.Controllers.Role
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Localization;
  using Moq;
  using Service.Interfaces;
  using System.Collections.Generic;
  using TemplateCore.Controllers;
  using ViewModels.Role;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Index of the class <see cref="RoleController"/>.
  /// </summary>
  public class Index
  {
    #region Private Fields

    /// <summary>
    /// The controller
    /// </summary>
    private RoleController controller = null;

    /// <summary>
    /// The localizer
    /// </summary>
    private Mock<IStringLocalizer<RoleController>> localizer = null;

    /// <summary>
    /// The role service
    /// </summary>
    private Mock<IRoleService> roleService = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> class.
    /// </summary>
    public Index()
    {
      this.roleService = new Mock<IRoleService>();

      this.localizer = new Mock<IStringLocalizer<RoleController>>();
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method Index of the class <see cref="RoleController"/>.
    /// Asserts the invoke of the method returns a List of type <see cref="AdminRoleIndexViewModel"/>.
    /// </summary>
    [Fact]
    public void IndexOk()
    {
      // setup
      var roleNames = new List<string>
      {
        "Test",
        "Administrator",
        "Payroll",
        "Execute reports"
      };

      this.roleService.Setup(x => x.GetAllRoleNames()).Returns(roleNames);
      this.controller = new RoleController(this.roleService.Object, this.localizer.Object);

      // action
      var result = (this.controller.Index() as ViewResult).Model as IEnumerable<AdminRoleIndexViewModel>;

      // assert
      Assert.IsType(typeof(List<AdminRoleIndexViewModel>), result);
    }

    #endregion Public Methods
  }
}