//-----------------------------------------------------------------------
// <copyright file="SendSmsAsync.cs" company="Without name">
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
  /// Class test that tests the method SendSmsAsync of <see cref="AuthMessageSender"/> class.
  /// </summary>
  public class SendSmsAsync
  {
    /// <summary>
    /// Sends the email asynchronous ok.
    /// </summary>
    /// <returns>Send sms.</returns>
    [Fact]
    public async Task SendSmsAsyncAsyncOk()
    {
      // setup
      var number = "11223344555";
      var message = "foo";
      var authMessageSenderMock = new Mock<AuthMessageSender>();

      // act
      await authMessageSenderMock.Object.SendSmsAsync(number, message);
    }
  }
}
