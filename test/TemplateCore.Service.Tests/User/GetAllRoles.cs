namespace TemplateCore.Service.Tests.User
{
  using Implement;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using Repository;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  /// <summary>
  /// Class Test that tests the method GetAllRoles of the class <see cref="UserService"/>. 
  /// </summary>
  public class GetAllRoles
  {
    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The user service
    /// </summary>
    private UserService userService = null;

    /// <summary>
    /// Test the method GetAll of the class <see cref="UserService"/>
    /// Assert the invoke of the method returns a list of the type <see cref="IdentityRole"/>.
    /// Assert there are 2 roles.
    /// </summary>
    [Fact]
    public void GetAllRolesOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action
      List<IdentityRole> result = this.userService.GetAllRoles().ToList();

      // assert
      Assert.IsType(typeof(List<IdentityRole>), result);
      Assert.True(result.Count == 2);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllRoles" /> class.
    /// Seeds the DbContext
    /// </summary>
    public GetAllRoles()
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
        // seed roles.
        var roleStore = new RoleStore<IdentityRole>(context);

        if (!context.Roles.Any(r => r.Name == "Administrator"))
        {
          var roleAdministrator = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
          roleStore.CreateAsync(roleAdministrator);
        }

        if (!context.Roles.Any(r => r.Name == "User"))
        {
          var roleAdministrator = new IdentityRole { Name = "User", NormalizedName = "User" };
          roleStore.CreateAsync(roleAdministrator);
        }

        context.SaveChangesAsync();
      }
    }
  }
}
