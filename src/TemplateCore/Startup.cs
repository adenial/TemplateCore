//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore
{
  using System.Globalization;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.AspNetCore.Mvc.Razor;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using TemplateCore.Model;
  using TemplateCore.Repository;
  using TemplateCore.Service.Implement;
  using TemplateCore.Service.Interfaces;
  using TemplateCore.Services;

  /// <summary>
  /// Class Startup.
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="env">The env.</param>
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      if (env.IsDevelopment())
      {
        // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
        builder.AddUserSecrets();
      }

      builder.AddEnvironmentVariables();
      this.Configuration = builder.Build();
    }

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <value>The configuration.</value>
    public IConfigurationRoot Configuration { get; }

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The env.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="seeder">The seeder.</param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TemplateDbContextSeedData seeder)
    {
      loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      /*loggerFactory
      .WithFilter(new FilterLoggerSettings
      {
        { "TemplateCore", LogLevel.Information }
      })
      .AddConsole();*/

      // add Trace Source logging
      /*var testSwitch = new SourceSwitch("sourceSwitch", "Logging");
      testSwitch.Level = SourceLevels.Warning;
      loggerFactory.AddTraceSource(testSwitch,
      new TextWriterTraceListener(writer: Console.Out));*/

      var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
      app.UseRequestLocalization(locOptions.Value);

      // error handling, while at development mode, show error page.
      if (env.IsDevelopment())
      {
        // app.UseDeveloperExceptionPage();
        // app.UseDatabaseErrorPage();
        app.UseBrowserLink();

        // status code pages
        // app.UseStatusCodePages();
        // app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseExceptionHandler("/Error/Error");
        app.UseStatusCodePagesWithReExecute("/Error/Errors/{0}");
      }
      else
      {
        // custom error page.
        // app.UseExceptionHandler("/Home/Error");

        // status code pages
        app.UseExceptionHandler("/Error/Error");
        app.UseStatusCodePagesWithReExecute("/Error/Errors/{0}");
      }

      app.UseStaticFiles();
      app.UseIdentity();

      // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });

      // seed database.
      seeder.Initialize();

      /*// Create a catch-all response
      app.Run(async (context) =>
      {
        if (context.Request.Path.Value.Contains("boom"))
        {
          throw new Exception("boom!");
        }
        var logger = loggerFactory.CreateLogger(typeof(UserController));
        logger.LogInformation("No endpoint found for request {path}", context.Request.Path);
        await context.Response.WriteAsync("No endpoint found - try /api/todo.");
      });*/
    }

    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="services">The services.</param>
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddDbContext<TemplateDbContext>(options =>
      options.UseSqlServer(this.Configuration.GetConnectionString("TemplateConnection")));
      services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TemplateDbContext>().AddDefaultTokenProviders();

      // Add the localization services to the services container
      services.AddLocalization(options => options.ResourcesPath = "Resources");
      services.AddMvc()

      // Add support for finding localized views, based on file name suffix, e.g. Index.es-MX.cshtml
      .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)

      // Add support for localizing strings in data annotations (e.g. validation messages) via the
      // IStringLocalizer abstractions.
      .AddDataAnnotationsLocalization();

      // Add application services.
      // services.AddTransient<TemplateDbContextSeedData>();
      services.AddTransient<IUnitOfWork<TemplateDbContext>, UnitOfWork<TemplateDbContext>>();
      services.AddTransient<IEmailSender, AuthMessageSender>();
      services.AddTransient<ISmsSender, AuthMessageSender>();
      services.AddTransient<IRoleService, RoleService>();
      services.AddTransient<IUserService, UserService>();
      this.AddPolicies(services);

      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new[]
        {
                    new CultureInfo("en-US"),
                    new CultureInfo("es-MX")
        };

        // State what the default culture for your application is. This will be used if no specific culture
        // can be determined for a given request.
        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

        // You must explicitly state which cultures your application supports.
        // These are the cultures the app supports for formatting numbers, dates, etc.
        options.SupportedCultures = supportedCultures;

        // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
        options.SupportedUICultures = supportedCultures;

        /*// You can change which providers are configured to determine the culture for requests, or even add a custom
        // provider with your own logic. The providers will be asked in order to provide a culture for each request,
        // and the first to provide a non-null result that is in the configured supported cultures list will be used.
        // By default, the following built-in providers are configured:
        // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
        // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
        // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
        //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
        //{
        //  // My custom request culture logic
        //  return new ProviderCultureResult("en");
        //}));*/
      });
    }

    /// <summary>
    /// Adds the policies.
    /// </summary>
    /// <param name="services">The services.</param>
    private void AddPolicies(IServiceCollection services)
    {
      // policies
      services.AddAuthorization(options =>
      {
        options.AddPolicy("View Administrator Menu", policy => policy.RequireRole("Administrator"));
      });
    }
  }
}