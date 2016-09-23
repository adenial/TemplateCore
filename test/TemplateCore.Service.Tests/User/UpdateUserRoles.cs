//-----------------------------------------------------------------------
// <copyright file="UpdateUserRoles.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Service.Tests.User
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using TemplateCore.Service.Interfaces;
  using Xunit;

  /// <summary>
  /// Class test that tests the method UpdateUserRoles of the class <see cref="UserService"/>
  /// </summary>
  public class UpdateUserRoles
  {
    /// <summary>
    /// The administrator role identifier
    /// </summary>
    private string administratorRoleId = string.Empty;

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    /// <summary>
    /// The reporter role identifier
    /// </summary>
    private string reporterRoleId = string.Empty;

    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    /// <summary>
    /// The user role identifier
    /// </summary>
    private string userRoleId = string.Empty;

    /// <summary>
    /// The user service
    /// </summary>
    private IUserService userService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserRoles"/> class.
    /// Seeds the database.
    /// </summary>
    public UpdateUserRoles()
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

        var administratorRole = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
        roleStore.CreateAsync(administratorRole);

        var executeReportsRole = new IdentityRole { Name = "Execute Reports Role", NormalizedName = "Execute Reports Role " };
        roleStore.CreateAsync(executeReportsRole);

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

        // after save changes, can use references of Ids.
        this.userId = user.Id;
        this.userRoleId = role.Id;
        this.administratorRoleId = administratorRole.Id;
        this.reporterRoleId = executeReportsRole.Id;
      }
    }

    /// <summary>
    /// Tests the method UpdateUserRoles of the class <see cref="UserService"/>.
    /// Assert the count of UserRoles of user after the invoke of the method, must be 2 roles.
    /// Assert the name of the roles.
    /// </summary>
    [Fact]
    public void UpdateUserRolesOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // create UserRoles to insert.
      var rolesToInsert = new List<IdentityUserRole<string>>
      {
        new IdentityUserRole<string> { RoleId = this.administratorRoleId, UserId = this.userId },
        new IdentityUserRole<string> { RoleId = this.reporterRoleId, UserId = this.userId }
      };

      // create UserRoles to delete.
      // the user role "User" is seeded at runtime (Constructor)
      var rolesToDelete = new List<IdentityUserRole<string>>
      {
        new IdentityUserRole<string> { RoleId = this.userRoleId, UserId = this.userId }
      };

      // action
      this.userService.UpdateUserRoles(rolesToInsert, rolesToDelete);

      // assert
      var roles = this.userService.GetRolesByUserId(this.userId).ToList();
      Assert.True(roles.Count == 2);
      Assert.True(roles[0].Name.Equals("Administrator"));
      Assert.True(roles[1].Name.Equals("Execute Reports Role"));
    }
  }
}