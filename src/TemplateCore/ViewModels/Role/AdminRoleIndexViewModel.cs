//-----------------------------------------------------------------------
// <copyright file="AdminRoleIndexViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Role
{
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class AdminRoleIndexViewModel.
  /// </summary>
  public class AdminRoleIndexViewModel
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Display(Name = "Name")]
    public string Name { get; set; }
  }
}
