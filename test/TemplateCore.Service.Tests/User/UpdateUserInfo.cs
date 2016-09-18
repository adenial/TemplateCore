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
  /// Class test that tests the method UpdateUserInfo of the class <see cref="UserService"/>
  /// </summary>
  public class UpdateUserInfo
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
    private IUserService userService = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserInfo"/> class.
    /// Seeds the database.
    /// </summary>
    public UpdateUserInfo()
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
        context.SaveChanges();
        this.userId = user.Id;
      }
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method UpdateUserInfo of the class <see cref="UserService"/>.
    /// Assert the name of the user was updated.
    /// </summary>
    [Fact]
    public void UpdateUserInfoOk()
    {
      // setup
      string editedName = "User for test purposes TEST";
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action
      this.userService.UpdateUserInfo(this.userId, editedName);

      var query = this.userService.GetAll();

      // assert
      Assert.Equal(editedName, query.ToList()[0].Name);
    }

    /// <summary>
    /// Tests the method UpdateUserInfo of the class <see cref="UserService"/>
    /// Assert the invoke of the method throws an exception of type <see cref="InvalidOperationException"/>.
    /// </summary>
    [Fact]
    public void UpdateUserInfoThrowsException()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action && assert
      Assert.Throws<InvalidOperationException>(() => this.userService.UpdateUserInfo(Guid.NewGuid().ToString(), "Test"));
    }

    #endregion Public Methods
  }
}