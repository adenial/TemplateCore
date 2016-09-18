namespace TemplateCore.Service.Tests.Role
{
  using Implement;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using Repository;
  using System;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="RoleService"/>.
  /// </summary>
  public class Constructor
  {
    #region Private Fields

    /// <summary>
    /// The role service
    /// </summary>
    private IRoleService roleService = null;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Tests the constructor of the class <see cref="RoleService"/>.
    /// Assert the invoke of the constructor returns an instance of the class.
    /// </summary>
    [Fact]
    public void RoleServiceOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(CreateNewContextOptions());
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);

      // action
      this.roleService = new RoleService(unitOfWork);

      // assert
      Assert.IsType(typeof(RoleService), this.roleService);
    }

    /// <summary>
    /// Tests the constructor of the class <see cref="RoleService"/>.
    /// Assert the invoke of the constructor throws an exception of type <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void RoleServiceThrowsExceptionDueParameter()
    {
      // setup, act and assert.
      Assert.Throws<ArgumentNullException>(() => this.roleService = new RoleService(null));
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Creates the new context options.
    /// </summary>
    /// <returns>DbContextOptions&lt;TemplateDbContext&gt;.</returns>
    private static DbContextOptions<TemplateDbContext> CreateNewContextOptions()
    {
      // Create a fresh service provider, and therefore a fresh
      // InMemory database instance.
      var serviceProvider = new ServiceCollection()
          .AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

      // Create a new options instance telling the context to use an
      // InMemory database and the new service provider.
      var builder = new DbContextOptionsBuilder<TemplateDbContext>();
      builder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);
      return builder.Options;
    }

    #endregion Private Methods
  }
}