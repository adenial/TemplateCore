namespace TemplateCore.ViewModels.User
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class UserCreateViewModel.
  /// </summary>
  public class UserCreateViewModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreateViewModel" /> class.
    /// </summary>
    public UserCreateViewModel()
    {
      this.Roles = new List<UserRoleCreateViewModel>();
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required(ErrorMessage = "The name is a required field.")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [Display(Name = "Username")]
    [Required(ErrorMessage = "The username is a required field.")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Display(Name = "Email")]
    [Required(ErrorMessage = "The email is a required field.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    public IEnumerable<UserRoleCreateViewModel> Roles { get; set; }
  }
}
