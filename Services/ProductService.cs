using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mtek_api.Data;
using mtek_api.Entities;
using MtekApi.Interfaces;

namespace MtekApi.Services
{
   public class ProductService : IProductService
   {
      //private CultureInfo th_TH = new CultureInfo("th-TH");
      private CultureInfo en_US = new CultureInfo("en-US");
      private string conStr = "";

      private string imgUrl = "";

      private readonly IConfiguration configuration;

      private readonly DatabaseContext dbContext;

      private readonly IUploadFileService uploadFileService;

      public ProductService(DatabaseContext dbContext, IUploadFileService uploadFileService, IConfiguration config)
      {
         this.dbContext = dbContext;
         this.uploadFileService = uploadFileService;
         this.configuration = config;
         conStr = configuration.GetConnectionString("ConnectionSqlServer");
         imgUrl = configuration.GetValue<string>("ImageUrl");
      }

      public async Task<List<ProductDto>> GetProductById(string pcd)
      {
         return (await dbContext.TbProducts.Where(p => p.Pcd == pcd).Include(p => p.GpcdNavigation).ToListAsync()).Select(ProductDto.FromTbProduct).ToList();
      }

      public async Task<List<ProductDto>> GetLocalProduct()
      {
         var ProductCusModel = new List<ProductDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ALL_INVENTORY";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  ProductCusModel.Add(new ProductDto
                  {
                     Pcd = dr["PCD"].ToString(),
                     Pdesc = dr["PDESC"].ToString(),
                     Gpcd = int.Parse(dr["GPCD"].ToString()),
                     Gpdesc = dr["GPDESC"].ToString(),
                     Uom = dr["UOM"].ToString(),
                     PrcCost = decimal.Parse(dr["PRC_COST"].ToString()),
                     PrcSale = decimal.Parse(dr["PRC_SALE"].ToString()),
                     Minstk = int.Parse(dr["MINSTK"].ToString()),
                     Stock = decimal.Parse(dr["STOCK"].ToString()),
                     Blqty = decimal.Parse(dr["BLQTY"].ToString()),
                     ImgPath = imgUrl + dr["IMG_PATH"].ToString(),
                     Lsactv = DateTime.Parse(dr["LASTV"].ToString(), en_US),
                  });
               }
               dr.Close();
            }
         }
         return ProductCusModel;
      }

      public async Task<List<ProductCusDto>> GetProductBranch(int cuscd, string pcd = "")
      {
         var ProductCusModel = new List<ProductCusDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ProductByCus";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cuscd", cuscd);
            cmd.Parameters.AddWithValue("@pcd", pcd);
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  ProductCusModel.Add(new ProductCusDto
                  {
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     FullName = dr["FULL_NAME"].ToString(),
                     Postcd = dr["POSTCD"].ToString(),
                     Pcd = dr["PCD"].ToString(),
                     Pdesc = dr["PDESC"].ToString(),
                     Gpcd = dr["GPCD"].ToString(),
                     Gpdesc = dr["GPDESC"].ToString(),
                     Uom = dr["UOM"].ToString(),
                     Stock = int.Parse(dr["STOCK"].ToString()),
                     Blqty = int.Parse(dr["BLQTY"].ToString()),
                     ImgPath = imgUrl + dr["IMG_PATH"].ToString(),
                     //Lsactv = DateTime.Parse(dr["LASTV"].ToString()),
                  });
               }
               dr.Close();
            }
         }
         return ProductCusModel;
      }

      public async Task<List<ProductCusDto>> GetAllProductBranch()
      {
         var ProductCusModel = new List<ProductCusDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_AllProductBranch";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  ProductCusModel.Add(new ProductCusDto
                  {
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     FullName = dr["FULL_NAME"].ToString(),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     Postcd = dr["POSTCD"].ToString(),
                     Pcd = dr["PCD"].ToString(),
                     Pdesc = dr["PDESC"].ToString(),
                     Gpcd = dr["GPCD"].ToString(),
                     Gpdesc = dr["GPDESC"].ToString(),
                     Uom = dr["UOM"].ToString(),
                     Stock = double.Parse(dr["STOCK"].ToString() == "" ? "0" : dr["STOCK"].ToString()),
                     Blqty = double.Parse(dr["BLQTY"].ToString() == "" ? "0" : dr["BLQTY"].ToString()),
                     ImgPath = imgUrl + dr["IMG_PATH"].ToString(),
                     //Lsactv = DateTime.Now
                     Lsactv = DateTime.Parse(dr["LASTV"].ToString(), en_US),
                  });
               }
               dr.Close();
            }
         }
         return ProductCusModel;
      }

      public async Task<List<ProductCusDto>> GetProductTrans(string pcd)
      {
         var ProductCusModel = new List<ProductCusDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ProductTrans";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@pcd", pcd);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  ProductCusModel.Add(new ProductCusDto
                  {
                     Billcd = dr["BILLCD"].ToString(),
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     FullName = dr["FULL_NAME"].ToString(),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     Postcd = dr["POSTCD"].ToString(),
                     Pcd = dr["PCD"].ToString(),
                     Pdesc = dr["PDESC"].ToString(),
                     Gpcd = dr["GPCD"].ToString(),
                     Gpdesc = dr["GPDESC"].ToString(),
                     Uom = dr["UOM"].ToString(),
                     Stock = double.Parse(dr["STOCK"].ToString() == "" ? "0" : dr["STOCK"].ToString()),
                     Blqty = double.Parse(dr["QTY"].ToString() == "" ? "0" : dr["QTY"].ToString()),
                     ImgPath = imgUrl + dr["IMG_PATH"].ToString(),
                     //Lsactv = DateTime.Now
                     Lsactv = DateTime.Parse(dr["LASTV"].ToString(), en_US),
                  });
               }
               dr.Close();
            }
         }
         return ProductCusModel;
      }

      public async Task<int> PostAddNewProduct(ProductRequestDtos model)
      {
         dbContext.TbProducts.Add(FromProductDto(model));
         return await dbContext.SaveChangesAsync();
      }

      public async Task<int> PutManageProduct(ProductRequestDtos model)
      {
         var product = await dbContext.TbProducts.FirstOrDefaultAsync(p => p.Pcd == model.Pcd);
         if (product != null)
         {
            product.Pdesc = model.Pdesc;
            product.Gpcd = model.GpCd;
            product.PrcCost = model.PrcCost;
            product.PrcSale = model.PrcSale;
            product.Uom = model.Uom;
            product.Stock = (int)model.Stock;
            product.Minstk = 0;
            if (model.ImgPath != null && model.ImgPath != "")
            {
               product.ImgPath = model.ImgPath;
            }

            return await dbContext.SaveChangesAsync();
         }
         throw new Exception("ไม่พบข้อมูลสินค้า");
      }

      public async Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles)
      {
         string errorMessage = String.Empty;
         string imageName = String.Empty;
         if (uploadFileService.IsUpload(formFiles))
         {
            errorMessage = uploadFileService.Validation(formFiles);
            if (String.IsNullOrEmpty(errorMessage))
            {
               imageName = (await uploadFileService.UploadImages(formFiles))[0];
            }
         }
         return (errorMessage, imageName);
      }

      public async Task<List<ProductGroupDtos>> GetProductGroup()
      {
         return (await dbContext.TbProductGroups.ToListAsync()).Select(ProductGroupDtos.FromTbProductGroup).ToList();
      }

      private TbProduct FromProductDto(ProductRequestDtos model)
      {
         return new TbProduct
         {
            Pcd = model.Pcd,
            Pdesc = model.Pdesc,
            Gpcd = model.GpCd,
            PrcCost = model.PrcCost,
            PrcSale = model.PrcSale,
            Uom = model.Uom,
            Stock = (int)model.Blqty,
            Minstk = 0,
            ImgPath = model.ImgPath
         };
      }

   }

}