namespace TemplateCore.Model
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using System;
  using System.Linq;

  /// <summary>
  /// Class TemplateDbContextSeedData.
  /// </summary>
  public class TemplateDbContextSeedData
  {
    #region Private Fields

    /// <summary>
    /// The password byt default
    /// </summary>
    private const string password = "1122334455";

    #endregion Private Fields

    #region Private Fields

    /// <summary>
    /// The context
    /// </summary>
    private TemplateDbContext context;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateDbContextSeedData"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TemplateDbContextSeedData(TemplateDbContext context)
    {
      this.context = context;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Initialize()
    {
      SeedAdminUser();
    }

    /// <summary>
    /// Seeds the admin user.
    /// </summary>
    private async void SeedAdminUser()
    {
      var adminUser = new ApplicationUser
      {
        Name = "Administrator User",
        UserName = "admin",
        NormalizedUserName = "admin",
        Email = "admin@test.com",
        NormalizedEmail = "admin@test.com",
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = Guid.NewGuid().ToString()
      };

      var roleStore = new RoleStore<IdentityRole>(context);

      if (!context.Roles.Any(r => r.Name == "Administrator"))
      {
        var roleAdministrator = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
        await roleStore.CreateAsync(roleAdministrator);
      }

      if (!context.Users.Any(u => u.UserName == adminUser.UserName))
      {
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashed = hasher.HashPassword(adminUser, password);
        adminUser.PasswordHash = hashed;
        var userStore = new UserStore<ApplicationUser>(context);
        await userStore.CreateAsync(adminUser);
        await userStore.AddToRoleAsync(adminUser, "Administrator");
      }

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
        await roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "User" });
      }

      if (!context.Users.Any(u => u.UserName == user.UserName))
      {
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashed = hasher.HashPassword(user, password);
        user.PasswordHash = hashed;
        var userStore = new UserStore<ApplicationUser>(context);
        await userStore.CreateAsync(user);
        await userStore.AddToRoleAsync(user, "User");
      }

      await context.SaveChangesAsync();
    }
    #endregion Public Methods
  }
}