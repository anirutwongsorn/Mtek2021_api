using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MtekApi.Installer
{
   public class CORSInstaller : IInstallers
   {
      public void InstallerService(IServiceCollection services, IConfiguration configuration)
      {
         services.AddCors(options =>
            {

               options.AddPolicy("AllowFrontend", builder =>
                   {
                      builder.WithOrigins("http://localhost:4200", "http://119.59.115.198:2021")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                   });

               //options.AddPolicy("AllowOrigin", options => options.AllowAnyHeader().AllowAnyOrigin().AllowCredentials());
            });
      }
   }
}