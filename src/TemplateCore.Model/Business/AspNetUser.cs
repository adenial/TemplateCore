using System;

namespace TemplateCore.Model
{
  /// <summary>
  /// Class AspNetUser.
  /// </summary>
  public class AspNetUser
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    public string Roles { get; set; }
  }
}
