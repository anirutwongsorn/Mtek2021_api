using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MtekApi.Installer;

namespace mtek_api
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.InstallServiceInAssembly(Configuration);
      }

      public void ConfigureContainer(ContainerBuilder builder)
      {
         builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(p => p.Name.EndsWith("Service")).AsImplementedInterfaces();
      }


      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "mtek_api v1"));
         }

         app.UseStaticFiles();// For the wwwroot folder

         app.UseStaticFiles(new StaticFileOptions
         {
            FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "images")),
            RequestPath = "/images"
         });

         //Enable directory browsing
         app.UseDirectoryBrowser(new DirectoryBrowserOptions
         {
            FileProvider = new PhysicalFileProvider(
                         Path.Combine(Directory.GetCurrentDirectory(), "images")),
            RequestPath = "/images"
         });

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseCors("AllowFrontend");

         app.UseAuthentication();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });
      }
   }
}
