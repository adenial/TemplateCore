namespace TemplateCore.Service
{
  using System.Collections.Generic;
  using System.Threading.Tasks;

  /// <summary>
  /// Interface IRoleService
  /// </summary>
  public interface IRoleService
  {
    /// <summary>
    /// Get all the role names.
    /// </summary>
    /// <returns>List of type <see cref="string"/>.</returns>
    IEnumerable<string> GetAllRoleNames();

    /// <summary>
    /// Determines whether this instance can insert the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if this instance can insert the specified name; otherwise, <c>false</c>.</returns>
    bool CanInsert(string name);

    /// <summary>
    /// Deletes a role with the specified name.
    /// </summary>
    /// <param name="name">The name of the role to delete.</param>
    void DeleteRole(string name);

    /// <summary>
    /// Inserts a new role with the specified name
    /// </summary>
    /// <param name="name">The name of the role to Insert.</param>
    Task Insert(string name);
  }
}
