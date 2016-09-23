//-----------------------------------------------------------------------
// <copyright file="UserRoleCreateViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.User
{
  /// <summary>
  /// Class UserRoleCreateViewModel.
  /// </summary>
  public class UserRoleCreateViewModel
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="UserRoleCreateViewModel"/> is check.
    /// </summary>
    /// <value><c>true</c> if check; otherwise, <c>false</c>.</value>
    public bool Check { get; set; }
  }
}
