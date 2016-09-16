namespace TemplateCore.Service
{
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Model;
  using Repository;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Class UserService.
  /// </summary>
  public class UserService : IUserService
  {
    /// <summary>
    /// The datacontext
    /// </summary>
    private TemplateDbContext dataContext = null;

    /// <summary>
    /// UnitOfWork
    /// </summary>
    private IUnitOfWork<TemplateDbContext> unitOfWork = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <exception cref="ArgumentNullException">dataContext</exception>
    public UserService(TemplateDbContext dataContext, IUnitOfWork<TemplateDbContext> unitOfWork)
    {
      if (dataContext == null)
      {
        throw new ArgumentNullException("dataContext");
      }

      if (unitOfWork == null)
      {
        throw new ArgumentNullException("unitOfWork");
      }

      this.dataContext = dataContext;
      this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Determines whether this instance [can insert email] the specified email.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns><c>true</c> if this instance [can insert email] the specified email; otherwise, <c>false</c>.</returns>
    public bool CanInsertEmail(string email)
    {
      bool canInsert = false;

      var query = this.dataContext.Users.Where(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();

      if (query == null)
      {
        // no data found.
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Determines whether this instance [can insert user name] the specified user name.
    /// </summary>
    /// <param name="userName">Name of the user.</param>
    /// <returns><c>true</c> if this instance [can insert user name] the specified user name; otherwise, <c>false</c>.</returns>
    public bool CanInsertUserName(string userName)
    {
      bool canInsert = false;

      var query = this.dataContext.Users.Where(x => x.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();

      if (query == null)
      {
        // no data found.
        canInsert = true;
      }

      return canInsert;
    }

    /// <summary>
    /// Deletes the user by its identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public void DeleteById(string id)
    {
      var user = this.dataContext.Users.Where(x => x.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
      this.dataContext.Users.Remove(user);
      this.dataContext.SaveChanges();
    }

    /// <summary>
    /// Exists the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public bool Exist(string id)
    {
      bool exists = false;

      var query = this.dataContext.Users.Where(x => x.Id == id).FirstOrDefault();

      if (query != null)
      {
        exists = true;
      }

      return exists;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>List of type <see cref="AspNetUser" />.</returns>
    public IEnumerable<AspNetUser> GetAll()
    {
      // if users are like a normal context...
      var users = this.dataContext.Users.ToList();

      return users.Select(x => new AspNetUser { Id = new Guid(x.Id), UserName = x.UserName, Name = x.Name }).ToList();
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <returns>List of type <see cref="IdentityRole"/>.</returns>
    public IEnumerable<IdentityRole> GetAllRoles()
    {
      return this.unitOfWork.RoleRepository.GetAll().ToList();
    }

    /// <summary>
    /// Inserts a new user.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="userName">Name of the user.</param>
    /// <param name="name">The name.</param>
    public void Insert(string email, string userName, string name)
    {
      // create new User
      ApplicationUser newUser = new ApplicationUser
      {
        // set data.
        Name = name,
        UserName = userName,
        NormalizedUserName = userName,
        Email = email,
        NormalizedEmail = email,
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = Guid.NewGuid().ToString()
      };

      // password by default.
      var password = new PasswordHasher<ApplicationUser>();
      var hashed = password.HashPassword(newUser, "1122334455");

      // forgot this.
      newUser.PasswordHash = hashed;

      // attach to context.
      this.dataContext.Users.Add(newUser);

      // save changes.
      this.dataContext.SaveChanges();
    }
  }
}
