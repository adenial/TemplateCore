//-----------------------------------------------------------------------
// <copyright file="Entity.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore.Model
{
  /// <summary>
  /// Class Entity.
  /// </summary>
  /// <typeparam name="T">Base Entity</typeparam>
  public abstract class Entity<T> : BaseEntity, IEntity<T>
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public virtual T Id { get; set; }
  }
}
