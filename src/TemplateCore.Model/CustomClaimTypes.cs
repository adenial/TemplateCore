//-----------------------------------------------------------------------
// <copyright file="CustomClaimTypes.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Model
{
  using System.Collections.Generic;
  using System.Security.Claims;

  /// <summary>
  /// Class CustomClaimTypes.
  /// </summary>
  public static class CustomClaimTypes
  {
    /// <summary>
    /// Gets or sets the permission.
    /// </summary>
    /// <value>The permission.</value>
    public static string Permission
    {
      get { return "Permission"; }
    }
  }
}
