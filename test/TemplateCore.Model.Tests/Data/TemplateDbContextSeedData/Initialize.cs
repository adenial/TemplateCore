//-----------------------------------------------------------------------
// <copyright file="Initialize.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Model.Tests.SeedDbContext
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Xunit;

  /// <summary>
  /// Class test that tests the method Initialize of <see cref="TemplateDbContextSeedData"/> class.
  /// </summary>
  public class Initialize
  {
    /// <summary>
    /// The context
    /// </summary>
    private TemplateDbContext context = null;

    /// <summary>
    /// The context options
    /// </summary>
    private DbContextOptions<TemplateDbContext> contextOptions = null;

    /// <summary>
    /// Tests the method Initialize.
    /// Assert the database is seeded.
    /// </summary>
    [Fact]
    public void InitializeOk()
    {
      // setup
      this.context = new TemplateDbContext(this.contextOptions);
      var seeder = new TemplateDbContextSeedData(context);

      // action
      seeder.Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Initialize"/> class.
    /// Initializes variables.
    /// </summary>
    public Initialize()
    {
      // Create a service provider to be shared by all test methods
      var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

      // Create options telling the context to use an
      // InMemory database and the service provider.
      var builder = new DbContextOptionsBuilder<TemplateDbContext>();
      builder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);
      this.contextOptions = builder.Options;
    }
  }
}
