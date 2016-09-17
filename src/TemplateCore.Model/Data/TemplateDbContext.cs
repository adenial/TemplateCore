namespace TemplateCore.Model
{
  using System;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Class TemplateDbContext.
  /// </summary>
  public class TemplateDbContext : IdentityDbContext<ApplicationUser>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
            : base(options)
    {
      // create db
      Database.EnsureCreated();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateDbContext"/> class.
    /// </summary>
    public TemplateDbContext()
      : base( new DbContextOptions<TemplateDbContext>())
    {
      // create db, lets wait till SetDbInitializer is implemented in EntityFramework Core.
      Database.EnsureCreated();
    }

    /// <summary>
    /// Configures the schema needed for the identity framework.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    /// <remarks>This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
    /// changes to entity instances before saving to the underlying database. This can be disabled via
    /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.</remarks>
    public override int SaveChanges()
    {
      return base.SaveChanges();
    }
  }
}
