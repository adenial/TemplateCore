//-----------------------------------------------------------------------
// <copyright file="Insert.cs" company="Without name">
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
  /// Class test that tests the method Insert of the class <see cref="Repository{TEntity}"/>.
  /// </summary>
  public class Insert
  {
    #region Private Fields

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions;

    private string roleId = string.Empty;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Insert"/> class.
    /// Seeds the database.
    /// </summary>
    public Insert()
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
        var roleStore = new RoleStore<IdentityRole>(context);
        var role = new IdentityRole { Name = "User", NormalizedName = "User" };
        roleStore.CreateAsync(role);
        context.SaveChangesAsync();
        this.roleId = role.Id;
      }
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Tests the method Insert of the class <see cref="Repository{TEntity}"/>.
    /// Happy path.
    /// Assert after the Insert of the user the count of the users incremented.
    /// </summary>
    [Fact]
    public void InsertOk()
    {
      // setup
      string password = "1122334455";
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      IUnitOfWork<TemplateDbContext> unitOfWork = new UnitOfWork<TemplateDbContext>(context);

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

      var hasher = new PasswordHasher<ApplicationUser>();
      var hashed = hasher.HashPassword(user, password);
      user.PasswordHash = hashed;

      // act
      int beforeInsert = unitOfWork.UserRepository.GetAll().ToList().Count;
      unitOfWork.UserRepository.Insert(user);
      unitOfWork.Commit();
      int afterInsert = unitOfWork.UserRepository.GetAll().ToList().Count;

      // assert
      Assert.True(afterInsert > beforeInsert);
    }

    /// <summary>
    /// Inserts the throws exception due parameter.
    /// Assert the invoke of the method throws an exception due parameter entity.
    /// </summary>
    [Fact]
    public void InsertThrowsExceptionDueParameter()
    {
      // setup
      TemplateDbContext context = new TemplateDbContext(this.contextOptions);
      var repository = new Repository<ApplicationUser>(context);

      // action
      var exception = Assert.Throws<ArgumentNullException>(() => repository.Insert(null));
      Assert.True(exception.Message.Equals("Value cannot be null.\r\nParameter name: entity"));
    }

    #endregion Public Methods
  }
}