using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MtekApi.Interfaces;

namespace MtekApi.Services
{
   public class UploadFIleService : IUploadFileService
   {
      private readonly IWebHostEnvironment webHostEnvironment;
      private readonly IConfiguration configuration;

      public UploadFIleService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
      {
         this.configuration = configuration;
         this.webHostEnvironment = webHostEnvironment;
      }

      public bool IsUpload(List<IFormFile> formFiles) => formFiles != null && formFiles.Sum(f => f.Length) > 0;

      public string Validation(List<IFormFile> formFiles)
      {
         foreach (var formFile in formFiles)
         {
            if (!ValidationExtension(formFile.FileName))
            {
               return "Invalid file extension";
            }

            if (!ValidationSize(formFile.Length))
            {
               return "The file is too large";
            }
         }
         return null;
      }

      public async Task<List<string>> UploadImages(List<IFormFile> formFiles)
      {
         List<string> listFileName = new List<string>();
         //string uploadPath = $"{webHostEnvironment.WebRootPath}/images/";
         string uploadPath = $"{Directory.GetCurrentDirectory()}/images/";

         foreach (var formFile in formFiles)
         {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string fullPath = uploadPath + fileName;
            using (var stream = File.Create(fullPath))
            {
               await formFile.CopyToAsync(stream);
            }
            listFileName.Add(fileName);
         }

         return listFileName;
      }

      public bool ValidationExtension(string fileName)
      {
         string[] permittedExtensions = { ".jpg", ".png" };
         var ext = Path.GetExtension(fileName).ToLowerInvariant();
         if (String.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
         {
            return false;
         }
         return true;
      }

      public bool ValidationSize(long fileSize) => configuration.GetValue<long>("FileSizeLimit") > fileSize;

   }

}