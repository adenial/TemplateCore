//-----------------------------------------------------------------------
// <copyright file="AdminRoleCreateViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Role
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  /// <summary>
  /// Class AdminRoleCreateViewModel.
  /// </summary>
  public class AdminRoleCreateViewModel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminRoleCreateViewModel" /> class.
    /// </summary>
    public AdminRoleCreateViewModel()
    {
      this.Claims = new List<RoleClaimCreateViewModel>();
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [Required(ErrorMessage = "The name is a required field.")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the claims.
    /// </summary>
    /// <value>The claims.</value>
    public IEnumerable<RoleClaimCreateViewModel> Claims { get; set; }
  }
}
