//-----------------------------------------------------------------------
// <copyright file="GetAllAsync.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Repository.Tests.User
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using Xunit;

  /// <summary>
  /// Class test that tests the method GetAllAsync of the <see cref="Repository{TEntity}"/> class.
  /// </summary>
  public class GetAllAsync
  {
    #region Private Fields

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions = null;

    /// <summary>
    /// The unit of work
    /// </summary>
    private IUnitOfWork<TemplateDbContext> unitOfWork = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FindBy"/> class.
    /// </summary>
    public GetAllAsync()
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

        var userTester = new ApplicationUser
        {
          Name = "User for test purposes",
          UserName = "tester",
          NormalizedUserName = "test",
          Email = "tester@test.com",
          NormalizedEmail = "tester@test.com",
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
          userTester.PasswordHash = hashed;
          var userStore = new UserStore<ApplicationUser>(context);
          userStore.CreateAsync(user);
          userStore.CreateAsync(userTester);
          userStore.AddToRoleAsync(user, "User");
          userStore.AddToRoleAsync(userTester, "User");
        }

        context.SaveChangesAsync();
      }
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the mthod GetAllAsync of the class <see cref="Repository{TEntity}"/>.
    /// Assert the invoke of the method returns a list object of the class <see cref="ApplicationUser"/>.
    /// Assert the list's count is 2.
    /// </summary>
    [Fact]
    public async Task GetAllAsyncOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);

      // act
      var user = await this.unitOfWork.UserRepository.GetAllAsync();

      // assert
      Assert.IsType(typeof(List<ApplicationUser>), user);
      Assert.True(user.ToList().Count == 2);
    }

    #endregion Public Methods
  }
}