//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.Role
{
  using System;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of the class <see cref="RoleService"/>.
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// The role service
    /// </summary>
    private IRoleService roleService = null;

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
      Assert.Throws<ArgumentNullException>(() => new RoleService(null));
    }

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
  }
}