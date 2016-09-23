//-----------------------------------------------------------------------
// <copyright file="VerifyPhoneNumberViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Manage
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class VerifyPhoneNumberViewModel.
  /// </summary>
  public class VerifyPhoneNumberViewModel
  {
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    /// <value>The phone number.</value>
    [Required]
    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
  }
}
