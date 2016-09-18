namespace TemplateCore.Service.Tests.User
{
  using Implement;
  using Interfaces;
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
  /// Class test that tests the method GetUserById of the class <see cref="UserService"/>
  /// </summary>
  public class GetUserById
  {
    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

    /// <summary>
    /// Test the method GetUserById of the class <see cref="UserService"/>.
    /// Assert the invoke of the method returns an instance of the class <see cref="ApplicationUser"/>.
    /// </summary>
    [Fact]
    public void GetUserByIdOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // act
      var result = this.userService.GetUserById(this.userId);

      // assert
      Assert.IsType(typeof(ApplicationUser), result);
    }

    /// <summary>
    /// Test the method GetUserById of the class <see cref="UserService"/>.
    /// Assert the invoke of the method throws an exception of the type <see cref="InvalidOperationException"/>.
    /// </summary>
    [Fact]
    public void GetUserByIdThrowsExceptionDueUserNotFound()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // act && assert
      Assert.Throws<InvalidOperationException>(() => this.userService.GetUserById(Guid.NewGuid().ToString()));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserById"/> class.
    /// Seeds the database.
    /// </summary>
    public GetUserById()
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

        var role = new IdentityRole { Name = "User", NormalizedName = "User" };
        roleStore.CreateAsync(role);

        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
          var hasher = new PasswordHasher<ApplicationUser>();
          var hashed = hasher.HashPassword(user, password);
          user.PasswordHash = hashed;
          var userStore = new UserStore<ApplicationUser>(context);
          userStore.CreateAsync(user);
        }

        context.SaveChangesAsync();
        context.UserRoles.Add(new IdentityUserRole<string> { RoleId = role.Id, UserId = user.Id });

        // save again.
        context.SaveChanges();
        this.userId = user.Id;
      }
    }
  }
}
