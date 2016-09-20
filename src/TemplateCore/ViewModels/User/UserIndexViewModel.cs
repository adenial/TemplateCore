namespace TemplateCore.ViewModels.User
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class UserIndexViewModel
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [Display(Name = "Usuario")]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Display(Name = "Nombre")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    [Display(Name = "Roles")]
    public string Roles { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [Display(Name = "Email")]
    public string Email { get; set; }
  }
}
