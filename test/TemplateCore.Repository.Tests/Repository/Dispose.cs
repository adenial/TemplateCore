namespace TemplateCore.Repository.Tests.Repository
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using System;
  using System.Linq;
  using TemplateCore.Model;
  using Xunit;

  /// <summary>
  /// Class tests that tests the Dispose method of <see cref="Repository{TEntity}<"/> class.
  /// </summary>
  public class Dispose : IDisposable
  {
    #region Private Fields

    /// <summary>
    /// The context
    /// </summary>
    private TemplateDbContext context = null;

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions = null;

    /// <summary>
    /// The user identifier
    /// </summary>
    private string userId = string.Empty;

    /// <summary>
    /// The user repository
    /// </summary>
    private Repository<ApplicationUser> userRepository = null;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FindBy"/> class.
    /// </summary>
    public Dispose()
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
    public void DisposeOk()
    {
      // setup
      this.context = new TemplateDbContext(this.contextOptions);
      this.userRepository = new Repository<ApplicationUser>(this.context);

      // act
      var user = this.userRepository.FindBy(x => x.Id.Equals(this.userId, StringComparison.CurrentCultureIgnoreCase));

      // assert
      Assert.IsType(typeof(ApplicationUser), user);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    void IDisposable.Dispose()
    {
      this.DisposeObjects(true);
      GC.SuppressFinalize(this);
    }

    #endregion Public Methods

    #region Protected Methods

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void DisposeObjects(bool disposing)
    {
      if (disposing)
      {
        this.userRepository.Dispose();
        this.context.Dispose();
      }
    }

    #endregion Protected Methods
  }
}