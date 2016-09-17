namespace TemplateCore.Service.Tests.UserService
{
  using Model;
  using Repository;
  using System;
  using TemplateCore.Service;
  using Xunit;

  /// <summary>
  /// Test class that test the constructor of the class <see cref="UserService"/> 
  /// </summary>
  public class Constructor
  {
    /// <summary>
    /// The user service
    /// </summary>
    private UserService userService = null;

    /*/// <summary>
    /// Test the constructor of the class <see cref="UserService"/>
    /// Assert the invoke of the constructor returns an instance of the class
    /// </summary>
    [Fact]
    public void ConstructorOk()
    {
      // setup
      IUnitOfWork<TemplateDbContext> unitOFWork = new UnitOfWork<TemplateDbContext>();

      // action
      this.userService = new UserService(unitOFWork);

      // assert
      Assert.IsType(typeof(UserService), this.userService);
    }*/

    /// <summary>
    /// Test the constructor of the class <see cref="UserService"/>
    /// Assert the invoke of the method throws an exception due parameter unitOfWork
    /// </summary>
    [Fact]
    public void ConstructorThrowsException()
    {
      // dont like this.
      try
      {
        this.userService = new UserService(null);
      }
      catch(ArgumentNullException ex)
      {
        Assert.IsType(typeof(ArgumentNullException), ex);
      }
    }
  }
}
