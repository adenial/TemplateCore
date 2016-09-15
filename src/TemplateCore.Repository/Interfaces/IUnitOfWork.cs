namespace TemplateCore.Repository
{
  using Microsoft.EntityFrameworkCore;
  using System;

  /// <summary>
  /// Interface IUnitOfWork
  /// </summary>
  /// <typeparam name="U"></typeparam>
  public interface IUnitOfWork<U> where U : DbContext, IDisposable
  {
    /// <summary>
    /// Saves all pending changes
    /// </summary>
    /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
    int Commit();
  }
}
