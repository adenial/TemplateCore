//-----------------------------------------------------------------------
// <copyright file="RemoveLoginViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Manage
{
  /// <summary>
  /// Class RemoveLoginViewModel.
  /// </summary>
  public class RemoveLoginViewModel
  {
    /// <summary>
    /// Gets or sets the login provider.
    /// </summary>
    /// <value>The login provider.</value>
    public string LoginProvider { get; set; }

    /// <summary>
    /// Gets or sets the provider key.
    /// </summary>
    /// <value>The provider key.</value>
    public string ProviderKey { get; set; }
  }
}
