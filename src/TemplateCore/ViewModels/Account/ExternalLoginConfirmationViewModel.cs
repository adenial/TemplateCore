//-----------------------------------------------------------------------
// <copyright file="ExternalLoginConfirmationViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Account
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class ExternalLoginConfirmationViewModel.
  /// </summary>
  public class ExternalLoginConfirmationViewModel
  {
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
