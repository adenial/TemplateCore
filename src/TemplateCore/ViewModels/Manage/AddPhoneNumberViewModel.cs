//-----------------------------------------------------------------------
// <copyright file="AddPhoneNumberViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Manage
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class AddPhoneNumberViewModel.
  /// </summary>
  public class AddPhoneNumberViewModel
  {
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
