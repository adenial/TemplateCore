using System.ComponentModel.DataAnnotations;

namespace TemplateCore.ViewModels.Role
{
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
