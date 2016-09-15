namespace TemplateCore.Model
{
  /// <summary>
  /// Interface IEntity
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IEntity<T>
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    T Id { get; set; }

    #endregion Public Properties
  }
}
