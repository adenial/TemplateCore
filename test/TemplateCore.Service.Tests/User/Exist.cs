//-----------------------------------------------------------------------
// <copyright file="Exist.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.User
{
  using System;
  using System.Linq;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Exist of the class <see cref="UserService"/>.
  /// </summary>
  public class Exist
  {
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
  }
}