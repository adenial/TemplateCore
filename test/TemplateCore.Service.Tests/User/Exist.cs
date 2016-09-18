namespace TemplateCore.Service.Tests.User
{
  using Implement;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Model;
  using Repository;
  using System;
  using System.Linq;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Exist of the class <see cref="UserService"/>.
  /// </summary>
  public class Exist
  {
    #region Private Fields

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    /// <summary>
    /// The user service
    /// </summary>
    private UserService userService = null;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Test the method Exist of the class <see cref="UserService"/>.
    /// Assert the invoke of the method returns True.
    /// </summary>
    [Fact]
    public void ExistTrue()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action
      var result = this.userService.Exist(this.userId);

      // assert
      Assert.True(result);
    }

    /// <summary>
    /// Test the method Exist of the class <see cref="UserService"/>.
    /// Assert the invoke of the method returns False.
    /// </summary>
    [Fact]
    public void ExistFalse()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action
      var result = this.userService.Exist(Guid.NewGuid().ToString());

      // assert
      Assert.False(result);
    }

    #endregion Public Methods

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Exist"/> class.
    /// Seeds the database.
    /// </summary>
    public Exist()
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
        string password = "1122334455";
        var roleStore = new RoleStore<IdentityRole>(context);

        var user = new ApplicationUser
        {
          Name = "User for test purposes",
          UserName = "test",
          NormalizedUserName = "test",
          Email = "test@test.com",
          NormalizedEmail = "test@test.com",
          EmailConfirmed = true,
          LockoutEnabled = false,
          SecurityStamp = Guid.NewGuid().ToString()
        };

        if (!context.Roles.Any(r => r.Name == "User"))
        {
          roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "User" });
        }

        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
          var hasher = new PasswordHasher<ApplicationUser>();
          var hashed = hasher.HashPassword(user, password);
          user.PasswordHash = hashed;
          var userStore = new UserStore<ApplicationUser>(context);
          userStore.CreateAsync(user);
          userStore.AddToRoleAsync(user, "User");
        }

        context.SaveChangesAsync();

        // check this.
        this.userId = user.Id;
      }
    }

    #endregion Public Constructors
  }
}