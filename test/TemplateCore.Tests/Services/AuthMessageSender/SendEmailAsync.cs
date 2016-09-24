//-----------------------------------------------------------------------
// <copyright file="SendEmailAsync.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Tests.Services.AuthMessageSender
{
  using System.Threading.Tasks;
  using Moq;
  using TemplateCore.Services;
  using Xunit;

  /// <summary>
  /// Class test that tests the method SendEmailAsync of <see cref="AuthMessageSender"/> class.
  /// </summary>
  public class SendEmailAsync
  {
    /// <summary>
    /// Sends the email asynchronous ok.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task SendEmailAsyncOk()
    {
      // setup
      var email = "test@test.com";
      var subject = "subject";
      var message = "foo";
      var authMessageSenderMock = new Mock<AuthMessageSender>();

      // act
      await authMessageSenderMock.Object.SendEmailAsync(email, subject, message);
    }
  }
}
