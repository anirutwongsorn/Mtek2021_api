using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MtekApi.Installer
{
   public interface IInstallers
   {
      void InstallerService(IServiceCollection services, IConfiguration configuration);
   }
}