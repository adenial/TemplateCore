//-----------------------------------------------------------------------
// <copyright file="ConfigureTwoFactorViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Manage
{
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Mvc.Rendering;

  /// <summary>
  /// Class ConfigureTwoFactorViewModel.
  /// </summary>
  public class ConfigureTwoFactorViewModel
  {
    /// <summary>
    /// Gets or sets the selected provider.
    /// </summary>
    /// <value>The selected provider.</value>
    public string SelectedProvider { get; set; }

    /// <summary>
    /// Gets or sets the providers.
    /// </summary>
    /// <value>The providers.</value>
    public ICollection<SelectListItem> Providers { get; set; }
  }
}
