using System.ComponentModel.DataAnnotations;

namespace TemplateCore.ViewModels.Manage
{
  public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
