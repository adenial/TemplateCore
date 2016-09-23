//-----------------------------------------------------------------------
// <copyright file="Delete.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Repository.Tests.User
{
  using System;
  using System.Linq;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using TemplateCore.Model;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Delete of the class <see cref="Repository{TEntity}"/> .
  /// </summary>
  public class Delete
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

    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Delete"/> class.
    /// </summary>
    public Delete()
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
        this.userId = user.Id;
      }
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method Delete of the class <see cref="Repository{TEntity}"/>.
    /// Asserts that the user count after deletion is less than before.
    /// </summary>
    [Fact]
    public void DeleteOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);

      // act
      int countBeforeDeletion = this.unitOfWork.UserRepository.GetAll().ToList().Count;
      var user = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(userId, StringComparison.CurrentCultureIgnoreCase));
      this.unitOfWork.UserRepository.Delete(user);
      this.unitOfWork.Commit();
      int countAfterDeletion = this.unitOfWork.UserRepository.GetAll().ToList().Count;

      // assert
      Assert.True(countBeforeDeletion > countAfterDeletion);
    }

    #endregion Public Methods
  }
}