//-----------------------------------------------------------------------
// <copyright file="FindBy.cs" company="Without name">
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
  /// Class test that tests the method FindBy of the <see cref="Repository{TEntity}"/> class.
  /// </summary>
  public class FindBy
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
    /// Initializes a new instance of the <see cref="FindBy"/> class.
    /// </summary>
    public FindBy()
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
    /// Tests the mthod FindBy of the class <see cref="Repository{TEntity}"/>.
    /// Assert the invoke of the method returns an object of the class <see cref="ApplicationUser"/>.
    /// </summary>
    [Fact]
    public void FindByOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);

      // act
      var user = this.unitOfWork.UserRepository.FindBy(x => x.Id.Equals(this.userId, StringComparison.CurrentCultureIgnoreCase));

      // assert
      Assert.IsType(typeof(ApplicationUser), user);
    }

    #endregion Public Methods
  }
}