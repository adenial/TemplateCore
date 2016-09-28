//-----------------------------------------------------------------------
// <copyright file="TemplateDbContextSeedData.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Model
{
  using System;
  using System.Linq;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using System.Security.Claims;

  /// <summary>
  /// Class TemplateDbContextSeedData.
  /// </summary>
  public class TemplateDbContextSeedData
  {
    /// <summary>
    /// The password byt default
    /// </summary>
    private const string Password = "1122334455";

    /// <summary>
    /// The context
    /// </summary>
    private TemplateDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateDbContextSeedData"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TemplateDbContextSeedData(TemplateDbContext context)
    {
      this.context = context;
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Initialize()
    {
      this.SeedAdminUser();
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

      var roleStore = new RoleStore<IdentityRole>(this.context);
      var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null, null);

      if (!this.context.Roles.Any(r => r.Name == "Administrator"))
      {
        var roleAdministrator = new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" };
        await roleStore.CreateAsync(roleAdministrator);
      }

      if (!this.context.Users.Any(u => u.UserName == adminUser.UserName))
      {
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashed = hasher.HashPassword(adminUser, Password);
        adminUser.PasswordHash = hashed;
        var userStore = new UserStore<ApplicationUser>(this.context);
        await userStore.CreateAsync(adminUser);
        await userStore.AddToRoleAsync(adminUser, "Administrator");
        await roleManager.AddClaimAsync(roleManager.Roles.ToList().Where(x => x.Name == "Administrator").FirstOrDefault(), new Claim(CustomClaimTypes.Permission, "Administrator.Menu"));
        await roleManager.AddClaimAsync(roleManager.Roles.ToList().Where(x => x.Name == "Administrator").FirstOrDefault(), new Claim(CustomClaimTypes.Permission, "Users.List"));
        await roleManager.AddClaimAsync(roleManager.Roles.ToList().Where(x => x.Name == "Administrator").FirstOrDefault(), new Claim(CustomClaimTypes.Permission, "Users.Create"));
        await roleManager.AddClaimAsync(roleManager.Roles.ToList().Where(x => x.Name == "Administrator").FirstOrDefault(), new Claim(CustomClaimTypes.Permission, "Users.Update"));
        await roleManager.AddClaimAsync(roleManager.Roles.ToList().Where(x => x.Name == "Administrator").FirstOrDefault(), new Claim(CustomClaimTypes.Permission, "Users.Delete"));
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

      if (!this.context.Roles.Any(r => r.Name == "User"))
      {
        await roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "User" });
      }

      if (!this.context.Users.Any(u => u.UserName == user.UserName))
      {
        var hasher = new PasswordHasher<ApplicationUser>();
        var hashed = hasher.HashPassword(user, Password);
        user.PasswordHash = hashed;
        var userStore = new UserStore<ApplicationUser>(this.context);
        await userStore.CreateAsync(user);
        await userStore.AddToRoleAsync(user, "User");
      }

      await this.context.SaveChangesAsync();
    }
  }
}