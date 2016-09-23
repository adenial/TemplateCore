//-----------------------------------------------------------------------
// <copyright file="AdminRoleCreateViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Role
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class AdminRoleCreateViewModel.
  /// </summary>
  public class AdminRoleCreateViewModel
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required(ErrorMessage = "The name is a required field.")]
    [Display(Name = "Name")]
    public string Name { get; set; }
  }
}
