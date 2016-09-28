//-----------------------------------------------------------------------
// <copyright file="RoleClaimCreateViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Role
{
  /// <summary>
  /// Class RoleClaimCreateViewModel.
  /// </summary>
  public class RoleClaimCreateViewModel
  {
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="RoleClaimCreateViewModel"/> is check.
    /// </summary>
    /// <value><c>true</c> if check; otherwise, <c>false</c>.</value>
    public bool Check { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public string Type { get; set; }
  }
}
