namespace TemplateCore.ViewModels.User
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class UserCreateViewModel.
  /// </summary>
  public class UserCreateViewModel
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required(ErrorMessage ="El nombre es un campo requerido.")]
    [Display(Name = "Nombre")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [Display(Name = "Cuenta de usuario")]
    [Required(ErrorMessage = "La cuenta de usuario es un campo requerido.")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "Formato de correo incorrecto.")]
    public string Email { get; set; }
  }
}
