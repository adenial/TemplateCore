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
    [Display(Name = "Nombre")]
    public string Name { get; set; }
  }
}
