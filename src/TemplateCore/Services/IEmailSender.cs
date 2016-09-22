﻿namespace TemplateCore.Services
{
  using System.Threading.Tasks;

  public interface IEmailSender
  {
    /// <summary>
    /// Sends the email asynchronous.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="subject">The subject.</param>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    Task SendEmailAsync(string email, string subject, string message);
  }
}
