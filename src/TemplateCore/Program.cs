//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Without name">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------
namespace TemplateCore
{
  using System.IO;
  using Microsoft.AspNetCore.Hosting;

  /// <summary>
  /// Class Program.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
      var host = new WebHostBuilder()
          .UseKestrel()
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseIISIntegration()
          .UseStartup<Startup>()
          .Build();
      host.Run();
    }
  }
}
