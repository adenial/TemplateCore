using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateCore.Model
{
  /// <summary>
  /// Class Entity.
  /// </summary>
  /// <typeparam name="T">Base Entity</typeparam>
  public abstract class Entity<T> : BaseEntity, IEntity<T>
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public virtual T Id { get; set; }

    #endregion Public Properties
  }
}
