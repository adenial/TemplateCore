namespace TemplateCore.Model
{
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

  /// <summary>
  /// Class ApplicationUser.
  /// </summary>
  public class ApplicationUser : IdentityUser
  {
    public string Name { get; set; }
  }
}
