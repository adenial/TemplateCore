namespace TemplateCore.ViewModels.Account
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class LoginViewModel.
  /// </summary>
  public class LoginViewModel
  {
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [Required(ErrorMessage = "The username is a required field.")]
    [Display(Name = "UserName")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Required(ErrorMessage = "The password is a required field.")]
    [Display(Name ="Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [remember me].
    /// </summary>
    /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
  }
}
