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
  /// Class test that tests the method Insert of the class <see cref="RoleService"/>.
  /// </summary>
  public class Insert
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
    /// Tests the method Insert of the class <see cref="RoleService"/>
    /// Assert the Role is inserted.
    /// </summary>
    [Fact]
    public void InsertOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.roleService = new RoleService(unitOfWork);

      // action
      int countBeforeInsert = this.roleService.GetAllRoleNames().ToList().Count;
      this.roleService.Insert("User");
      int countAfterInsert = this.roleService.GetAllRoleNames().ToList().Count;

      // assert
      Assert.True(countBeforeInsert < countAfterInsert);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Insert"/> class.
    /// Seeds the database.
    /// </summary>
    public Insert()
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
        if (!context.Roles.Any(r => r.Name == "Administrator"))
        {
          roleStore.CreateAsync(new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" });
        }

        context.SaveChangesAsync();
      }
    }
  }
}
