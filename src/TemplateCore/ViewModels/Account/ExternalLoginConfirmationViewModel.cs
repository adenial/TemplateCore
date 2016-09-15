using System.ComponentModel.DataAnnotations;

namespace TemplateCore.ViewModels.Account
{
  public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
