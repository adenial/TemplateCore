//-----------------------------------------------------------------------
// <copyright file="DeleteById.cs" company="Without name">
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
  /// Test class that test the method DeleteById of the class <see cref="UserService"/>.
  /// </summary>
  public class DeleteById
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
    /// Initializes a new instance of the <see cref="DeleteById"/> class.
    /// Seeds the context and share it with all the tests within the class.
    /// </summary>
    public DeleteById()
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
    /// Test the method DeleteById of the class <see cref="UserService"/>.
    /// Assert the record was deleted?
    /// </summary>
    [Fact]
    public void DeleteByIdOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      // action
      int countBeforeDeletion = this.userService.GetAll().ToList().Count;
      this.userService.DeleteById(this.userId);
      int countAfterDeletion = this.userService.GetAll().ToList().Count;

      // assert
      Assert.True(countAfterDeletion < countBeforeDeletion);
    }

    /// <summary>
    /// Test the method DeleteById of the class <see cref="UserService"/>.
    /// Assert the invoke of the method throws an exception due the user is not found.
    /// </summary>
    [Fact]
    public void DeleteByIdThrowsException()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);
      this.userService = new UserService(unitOfWork);

      string userId = Guid.NewGuid().ToString();

      Exception exception = Assert.Throws<InvalidOperationException>(() => this.userService.DeleteById(userId));

      // action && assert
      Assert.True(exception.Message.Equals(string.Format("Usert not found with the provided Id, Id provided: {0}", userId)));
    }
  }
}