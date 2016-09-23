//-----------------------------------------------------------------------
// <copyright file="IEmailSender.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Services
{
  using System.Threading.Tasks;

  /// <summary>
  /// Interface IEmailSender
  /// </summary>
  public interface IEmailSender
  {
    /// <summary>
    /// Sends the email asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="message">The message.</param>
    /// <returns>Sends Email.</returns>
    Task SendEmailAsync(string email, string subject, string message);
  }
}
