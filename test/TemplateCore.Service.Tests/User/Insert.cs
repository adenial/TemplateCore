namespace TemplateCore.Service.Tests.User
{
  using Implement;
  using Interfaces;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using Repository;
  using System.Collections.Generic;
  using System.Linq;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Insert of the class <see cref="UserService"/>.
  /// </summary>
  public class Insert
  {
    private string roleId = string.Empty;
    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

    /// <summary>
    /// Tests the method Insert of the class <see cref="UserService"/>.
    /// Happy path.
    /// Assert after the Insert of the user the count of the users incremented.
    /// </summary>
    [Fact]
    public void InsertOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // act
      int beforeInsert = this.userService.GetAll().ToList().Count;
      this.userService.Insert("test@test.com", "tester", "User for test purposes", new List<string> { this.roleId });
      int afterInsert = this.userService.GetAll().ToList().Count;

      // assert
      Assert.True(afterInsert > beforeInsert);
    }

    /// <summary>
    /// Tests the method Insert of the class <see cref="UserService"/>.
    /// This test case tests when no roles are specified (empty list).
    /// Assert after the Insert of the user the count of the users incremented.
    /// </summary>
    [Fact]
    public void InsertOkWithoutRoles()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // act
      int beforeInsert = this.userService.GetAll().ToList().Count;
      this.userService.Insert("test@test.com", "tester", "User for test purposes", new List<string>());
      int afterInsert = this.userService.GetAll().ToList().Count;

      // assert
      Assert.True(afterInsert > beforeInsert);
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
        var role = new IdentityRole { Name = "User", NormalizedName = "User" };
        roleStore.CreateAsync(role);
        context.SaveChangesAsync();
        this.roleId = role.Id;
      }
    }
  }
}
