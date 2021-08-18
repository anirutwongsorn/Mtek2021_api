using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MtekApi.Installer
{
   public class ControllerInstaller : IInstallers
   {
      public void InstallerService(IServiceCollection services, IConfiguration configration)
      {
         services.AddControllers();
      }
   }
}