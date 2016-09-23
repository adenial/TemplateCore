//-----------------------------------------------------------------------
// <copyright file="UserEditViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.User
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class UserEditViewModel.
  /// </summary>
  public class UserEditViewModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEditViewModel"/> class.
    /// </summary>
    public UserEditViewModel()
    {
      this.Roles = new List<UserRoleCreateViewModel>();
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Display(Name="Name")]
    [Required(ErrorMessage = "The name is a required field.")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    public IEnumerable<UserRoleCreateViewModel> Roles { get; set; }
  }
}
