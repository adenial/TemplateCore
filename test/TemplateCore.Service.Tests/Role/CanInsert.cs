//-----------------------------------------------------------------------
// <copyright file="CanInsert.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.Role
{
  using System.Linq;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the method CanInsert of the class <see cref="RoleService"/>
  /// </summary>
  public class CanInsert
  {
    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The role service
    /// </summary>
    private IRoleService roleService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="CanInsert"/> class.
    /// Seeds the data.
    /// </summary>
    public CanInsert()
    {
      // Create a service provider to be shared by all test methods
      var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

      // Create options telling the context to use an
      // InMemory database and the service provider.
      var builder = new DbContextOptionsBuilder<TemplateDbContext>();
      builder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);
      this.contextOptions = builder.Options;

      // seed in constructor.
      using (var context = new TemplateDbContext(this.contextOptions))
      {
        var roleStore = new RoleStore<IdentityRole>(context);
        if (!context.Roles.Any(r => r.Name == "User"))
        {
          roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "User" });
        }

        context.SaveChangesAsync();
      }
    }

    /// <summary>
    /// Tests the method CanInsert of the class <see cref="RoleService"/>
    /// Assert the invoke of method returns False
    /// </summary>
    [Fact]
    public void CanInsertFalse()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action
      var result = this.roleService.CanInsert("User");

      // assert
      Assert.False(result);
    }

    /// <summary>
    /// Tests the method CanInsert of the class <see cref="RoleService"/>
    /// Assert the invoke of method returns True
    /// </summary>
    [Fact]
    public void CanInsertTrue()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action
      var result = this.roleService.CanInsert("Administrator");

      // assert
      Assert.True(result);
    }
  }
}