namespace TemplateCore.Service.Tests
{
  using System;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Repository;
  using TemplateCore.Model;
  using Xunit;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using System.Linq;
  using Implement;

  /// <summary>
  /// Test Class that test the method CanInsertEmail of the service class <see cref="UserService" /></summary>
  public class CanInsertEmail
  {
    #region Private Fields

    /// <summary>
    /// The unit of work
    /// </summary>
    private IUnitOfWork<TemplateDbContext> unitOfWork = null;

    /// <summary>
    /// The user service
    /// </summary>
    private UserService userService = null;

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    /// Test the method CanInsertEmail of the class <see cref="UserService"/>
    /// Assert the invoke of the method returns false.
    /// </summary>
    [Fact]
    public void CanInsertMailFalse()
    {
      // setup
      var options = CreateNewContextOptions();
      var context = new TemplateDbContext(options);

      SeedData(context);
      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(this.unitOfWork);

      // action
      var result = this.userService.CanInsertEmail("admin@test.com");

      // assert
      Assert.False(result);
    }

    /// <summary>
    /// Test the method CanInsertEmail of the class <see cref="UserService"/>
    /// Assert the invoke of the method returns true.
    /// </summary>
    [Fact]
    public void CanInsertMailTrue()
    {
      // setup
      var options = CreateNewContextOptions();
      var context = new TemplateDbContext(options);

      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(this.unitOfWork);

      // action
      var result = this.userService.CanInsertEmail("admin@test.com");

      // assert
      Assert.True(result);
    }

    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void SeedData(TemplateDbContext context)
    {
      var password = "1122334455";
      var adminUser = new ApplicationUser
      {
        Name = "Administrator User",
        UserName = "admin",
        NormalizedUserName = "admin",
        Email = "admin@test.com",
        NormalizedEmail = "admin@test.com",
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = Guid.NewGuid().ToString()
      };

      var roleStore = new RoleStore<IdentityRole>(context);

      if (!context.Roles.Any(r => r.Name == "Administrator"))
      {
        var roleAdministrator = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
        roleStore.CreateAsync(roleAdministrator);
      }

      if (!context.Users.Any(u => u.UserName == adminUser.UserName))
      {
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashed = hasher.HashPassword(adminUser, password);
        adminUser.PasswordHash = hashed;
        var userStore = new UserStore<ApplicationUser>(context);
        userStore.CreateAsync(adminUser);
        userStore.AddToRoleAsync(adminUser, "Administrator");
      }

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
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Creates the new context options.
    /// </summary>
    /// <returns>DbContextOptions&lt;TemplateDbContext&gt;.</returns>
    private static DbContextOptions<TemplateDbContext> CreateNewContextOptions()
    {
      // Create a fresh service provider, and therefore a fresh
      // InMemory database instance.
      var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

      // Create a new options instance telling the context to use an
      // InMemory database and the new service provider.
      var builder = new DbContextOptionsBuilder<TemplateDbContext>();
      builder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);


      return builder.Options;
    }

    #endregion Private Methods
  }
}