//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Model.Tests.SeedDbContext
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of <see cref="TemplateDbContextSeedData"/> class.
  /// </summary>
  public class Constructor
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
    /// Initializes a new instance of the <see cref="Constructor"/> class.
    /// </summary>
    public Constructor()
    {
      // Create a service provider to be shared by all test methods
      var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

      // Create options telling the context to use an
      // InMemory database and the service provider.
      var builder = new DbContextOptionsBuilder<TemplateDbContext>();
      builder.UseInMemoryDatabase().UseInternalServiceProvider(serviceProvider);
      this.contextOptions = builder.Options;
    }

    /// <summary>
    /// Tests the constructor of <see cref="TemplateDbContextSeedData"/> class
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      this.context = new TemplateDbContext(contextOptions);

      // action
      var result = new TemplateDbContextSeedData(this.context);

      // assert
      Assert.IsType(typeof(TemplateDbContextSeedData), result);
    }
  }
}
