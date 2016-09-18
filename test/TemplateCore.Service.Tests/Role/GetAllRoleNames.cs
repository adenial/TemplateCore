namespace TemplateCore.Service.Tests.Role
{
  using Implement;
  using Interfaces;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using Repository;
  using System.Linq;
  using Xunit;

  /// <summary>
  /// Class test that tests the method GetAllRoleNames of the class <see cref="RoleService"/>.
  /// </summary>
  public class GetAllRoleNames
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
    /// Tests the method GetAllRoleNames of the class <see cref="RoleService"/>
    /// Assert the invoke of the method returns a List of type string with 3 elements.
    /// </summary>
    [Fact]
    public void GetAllRoleNamesOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action
      var result = this.roleService.GetAllRoleNames();

      // assert
      Assert.True(result.ToList().Count == 3);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllRoleNames"/> class.
    /// Seeds the database.
    /// </summary>
    public GetAllRoleNames()
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

        if (!context.Roles.Any(r => r.Name == "Administrator"))
        {
          roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
        }

        if (!context.Roles.Any(r => r.Name == "Execute Reporters"))
        {
          roleStore.CreateAsync(new IdentityRole { Name = "Execute Reporters", NormalizedName = "Execute Reporters" });
        }

        context.SaveChangesAsync();
      }
    }
  }
}
