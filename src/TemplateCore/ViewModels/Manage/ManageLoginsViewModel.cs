//-----------------------------------------------------------------------
// <copyright file="ManageLoginsViewModel.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.ViewModels.Manage
{
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Http.Authentication;
  using Microsoft.AspNetCore.Identity;

  /// <summary>
  /// Class ManageLoginsViewModel.
  /// </summary>
  public class ManageLoginsViewModel
  {
    /// <summary>
    /// Gets or sets the current logins.
    /// </summary>
    /// <value>The current logins.</value>
    public IList<UserLoginInfo> CurrentLogins { get; set; }

    /// <summary>
    /// Gets or sets the other logins.
    /// </summary>
    /// <value>The other logins.</value>
    public IList<AuthenticationDescription> OtherLogins { get; set; }
  }
}
