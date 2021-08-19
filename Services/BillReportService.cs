using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MtekApi.Dtos;
using MtekApi.Interfaces;

namespace MtekApi.Services
{
   public class BillReportService : IBillReportService
   {
      private readonly IConfiguration configuration;
      private string conStr = "", imgUrl = "";

      private CultureInfo en_US = new CultureInfo("en-US");

      public BillReportService(IConfiguration config)
      {
         this.configuration = config;
         conStr = configuration.GetConnectionString("ConnectionSqlServer");
         imgUrl = configuration.GetValue<string>("ImageUrl");
      }

      public async Task<List<PosBillDto>> GetAllBillMain()
      {
         var model = new List<PosBillDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ALL_BILL_MAIN";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  model.Add(new PosBillDto
                  {
                     BillCd = dr["BILLCD"].ToString(),
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     FullName = dr["FULL_NAME"].ToString(),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     GAmnt = decimal.Parse(dr["G_AMT"].ToString()),
                     GDiscnt = decimal.Parse(dr["G_DISCNT"].ToString()),
                     GTotal = decimal.Parse(dr["G_TOTAL"].ToString()),
                     Actv = DateTime.Parse(dr["BILLDT"].ToString(), en_US),
                     Remark = dr["REMARK"].ToString(),
                  });
               }
               dr.Close();
            }
         }
         return model;
      }

      public async Task<List<PosBillDto>> GetAllBillMainDetails(string billCd)
      {
         var model = new List<PosBillDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ALL_BILL_MAIN_DETAILS_BY_BILLCD";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@billCd", billCd);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  model.Add(new PosBillDto
                  {
                     BillCd = dr["BILLCD"].ToString(),
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     FullName = dr["FULL_NAME"].ToString(),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     GAmnt = decimal.Parse(dr["G_AMT"].ToString()),
                     GDiscnt = decimal.Parse(dr["G_DISCNT"].ToString()),
                     GTotal = decimal.Parse(dr["G_TOTAL"].ToString()),
                     Actv = DateTime.Parse(dr["BILLDT"].ToString(), en_US),
                     Remark = dr["REMARK"].ToString(),
                     Pcd = dr["PCD"].ToString(),
                     Pdesc = dr["PDESC"].ToString(),
                     Prcs = decimal.Parse(dr["PRCS"].ToString()),
                     Qty = decimal.Parse(dr["QTY"].ToString()),
                     Amount = decimal.Parse(dr["AMT"].ToString()),
                     Uom = dr["UOM"].ToString(),
                     ImgPath = imgUrl + dr["IMG_PATH"].ToString(),
                  });
               }
               dr.Close();
            }
         }
         return model;
      }

   }
}