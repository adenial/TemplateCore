/// <summary>
/// The TemplateDbContext namespace.
/// </summary>
namespace TemplateCore.Model.Tests.DbContext
{
  using TemplateCore.Model;
  using Xunit;

  /// <summary>
  /// Class test that tests the constructor of <see cref="TemplateDbContext"/> class
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// Tests the constructor of <see cref="TemplateDbContext"/> class
    /// Asserts the invoke of the constructor return an instance of the class.
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // action
      var dbContext = new TemplateDbContext();
      
      // assert
      Assert.IsType(typeof(TemplateDbContext), dbContext);
    }
  }
}
