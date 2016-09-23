//-----------------------------------------------------------------------
// <copyright file="DeleteRoleByName.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.Role
{
  using System;
  using System.Linq;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the method DeleteRoleByName of the class <see cref="RoleService"/>.
  /// </summary>
  public class DeleteRoleByName
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
    /// Initializes a new instance of the <see cref="DeleteRoleByName"/> class.
    /// Seeds the database.
    /// </summary>
    public DeleteRoleByName()
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
    /// Tests the method DeleteRoleByName of the class <see cref="RoleService"/>.
    /// Assert the role was deleted.
    /// Happy path.
    /// </summary>
    [Fact]
    public void DeleteRoleByNameOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action
      int countBeforeDeletetion = this.roleService.GetAllRoleNames().ToList().Count;
      this.roleService.DeleteRoleByName("User");
      int countAfterDeletion = this.roleService.GetAllRoleNames().ToList().Count;

      // assert
      Assert.True(countBeforeDeletetion > countAfterDeletion);
    }

    /// <summary>
    /// Tests the method DeleteRoleByName of the class <see cref="RoleService"/>.
    /// Assert the invoke of the method throws an exception of type <see cref="ArgumentNullException"/>
    /// </summary>
    [Fact]
    public void DeleteRoleByNameThrowsExceptionDueParameter()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action & assert
      Assert.Throws<ArgumentNullException>(() => this.roleService.DeleteRoleByName(string.Empty));
    }

    /// <summary>
    /// Tests the method DeleteRoleByName of the class <see cref="RoleService"/>.
    /// Assert the invoke of the method throws an exception of type <see cref="ArgumentNullException"/>
    /// Tests the case when the role is not found with the provided Id.
    /// </summary>
    [Fact]
    public void DeleteRoleByNameThrowsExceptionDueRoleNotFound()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action & assert
      Assert.Throws<ArgumentNullException>(() => this.roleService.DeleteRoleByName("Administrator"));
    }
  }
}