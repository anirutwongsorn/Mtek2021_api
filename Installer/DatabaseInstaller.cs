using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mtek_api.Data;

namespace MtekApi.Installer
{
   public class DatabaseInstaller : IInstallers
   {
      public void InstallerService(IServiceCollection services, IConfiguration configuration)
      {
         // using Microsoft.EntityFrameworkCore;
         services.AddDbContext<DatabaseContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("ConnectionSqlServer")));

      }
   }
}