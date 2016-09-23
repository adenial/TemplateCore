//-----------------------------------------------------------------------
// <copyright file="ForgotPasswordViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Account
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class ForgotPasswordViewModel.
  /// </summary>
  public class ForgotPasswordViewModel
  {
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [Required]
    public string Username { get; set; }
  }
}
