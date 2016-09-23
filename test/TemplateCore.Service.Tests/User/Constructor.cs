//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.User
{
  using System;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using Xunit;

  /// <summary>
  /// Test class that test the constructor of the class <see cref="UserService"/>
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// The user service
    /// </summary>
    private UserService userService = null;

    /// <summary>
    /// Test the constructor of the class <see cref="UserService"/>
    /// Assert the invoke of the constructor returns an instance of the class
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      var options = CreateNewContextOptions();
      var context = new TemplateDbContext(options);
      IUnitOfWork<TemplateDbContext> unitOFWork = new UnitOfWork<TemplateDbContext>(context);

      // action
      this.userService = new UserService(unitOFWork);

      // assert
      Assert.IsType(typeof(UserService), this.userService);
    }

    /// <summary>
    /// Test the constructor of the class <see cref="UserService"/>
    /// Assert the invoke of the method throws an exception due parameter unitOfWork
    /// </summary>
    [Fact]
    public void ConstructorThrowsException()
    {
      // setup, action & assert
      Assert.Throws<ArgumentNullException>(() => new UserService(null));
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