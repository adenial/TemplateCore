namespace TemplateCore.Repository.Tests.User
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using TemplateCore.Model;
  using Xunit;

  /// <summary>
  /// Class test that tests the method FindByAsync of the <see cref="Repository{TEntity}"/> class.
  /// </summary>
  public class FindByAsync
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
    public FindByAsync()
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
    /// Tests the mthod FindByAsync of the class <see cref="Repository{TEntity}"/>.
    /// Assert the invoke of the method returns an object of the class <see cref="ApplicationUser"/>.
    /// </summary>
    [Fact]
    public async Task FindByAsyncOk()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      this.unitOfWork = new UnitOfWork<TemplateDbContext>(context);

      // act
      var user = await this.unitOfWork.UserRepository.FindByAsync(x => x.Id.Equals(this.userId, StringComparison.CurrentCultureIgnoreCase));

      // assert
      Assert.IsType(typeof(ApplicationUser), user);
    }

    #endregion Public Methods
  }
}