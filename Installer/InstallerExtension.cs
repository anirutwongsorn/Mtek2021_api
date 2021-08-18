using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mtek_api;

namespace MtekApi.Installer
{
   public static class InstallerExtension
   {
      public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
      {
         var installers = typeof(Startup).Assembly.ExportedTypes.Where(p =>
         typeof(IInstallers).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
         .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();
         installers.ForEach(i => i.InstallerService(services, configuration));
      }
   }
}