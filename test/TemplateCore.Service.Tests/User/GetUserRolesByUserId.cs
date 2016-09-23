//-----------------------------------------------------------------------
// <copyright file="GetUserRolesByUserId.cs" company="Without name">
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
  /// Class test that tests the method GetUserRolesByUserId of the class <see cref="UserService"/>.
  /// </summary>
  public class GetUserRolesByUserId
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
    private IUserService userService = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserRolesByUserId"/> class.
    /// </summary>
    public GetUserRolesByUserId()
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

    /// <summary>
    /// Tests the method GetUserRolesByUserId of the class <see cref="UserService"/>
    /// Assert the invoke of the method returns a List of the type <see cref="IdentityUserRole{TKey}"/>
    /// </summary>
    [Fact]
    public void GetUserRolesByUserIdOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // act
      var result = this.userService.GetUserRolesByUserId(this.userId);

      // assert
      Assert.IsType(typeof(List<IdentityUserRole<string>>), result);
    }
  }
}