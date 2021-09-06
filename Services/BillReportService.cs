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
using mtek_api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MtekApi.Services
{
   public class BillReportService : IBillReportService
   {
      private readonly IConfiguration configuration;

      private readonly DatabaseContext dbContext;

      private string conStr = "", imgUrl = "";

      private CultureInfo en_US = new CultureInfo("en-US");

      public BillReportService(IConfiguration config, DatabaseContext dbContext)
      {
         this.configuration = config;
         this.dbContext = dbContext;
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

      public async Task<List<SaleReportDto>> GetSaleReportByCus(DateTime dateFrom, DateTime dateTo, int cusid)
      {
         var saleModel = new List<SaleReportDto>();
         saleModel = (await dbContext.TbBillHeaders
                     .Where(p => p.CusId == cusid && p.Actv.Value.Date >= dateFrom.Date && p.Actv.Value.Date <= dateTo.Date)
                     .Include(c => c.Cus)
                     .Include(p => p.TbBillWos)
                     .Include(p => p.TbBillWos)
                     .ToListAsync()).Select(SaleReportDto.FromBillHeader).ToList();
         return saleModel;
      }

      public async Task<List<ProductCusDto>> GetSaleBillMainByCusid(int cusid)
      {
         var saleModel = new List<ProductCusDto>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_ALL_BILL_MAIN_CUS";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cusid", cusid);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  var _billcd = dr["BillCd"].ToString();
                  if (_billcd.StartsWith("TG")) { continue; }
                  saleModel.Add(new ProductCusDto
                  {
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     Billcd = _billcd,
                     ShopName = dr["SHOP_NAME"].ToString(),
                     FullName = dr["FULL_NAME"].ToString(),
                     AddressNo = dr["ADDRESS_NO"].ToString(),
                     Amount = double.Parse(dr["G_TOTAL"].ToString()),
                     Remark = dr["REMARK"].ToString(),
                     Lsactv = DateTime.Parse(dr["BILLDT"].ToString(), en_US),
                     //Updated = dr["LASTV"].ToString(),
                  });
               }
               dr.Close();
            }
         }
         return saleModel;
      }

      public async Task<List<SaleReportCusDtos>> GetSaleReportWithCommission()
      {
         var saleModel = new List<SaleReportCusDtos>();
         using (var conn = new SqlConnection(conStr))
         {
            SqlDataReader dr;
            var sql = "GET_CUSTOMER_WITH_COMMISSION";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (conn.State == ConnectionState.Closed) { await conn.OpenAsync(); }
            dr = await cmd.ExecuteReaderAsync();
            if (dr.HasRows)
            {
               while (dr.Read())
               {
                  int _commission = int.Parse(dr["Commission"].ToString());
                  double _amount = double.Parse(dr["Amount"].ToString());
                  double _income = Math.Round(((double)_commission / 100) * _amount);
                  saleModel.Add(new SaleReportCusDtos
                  {
                     CusId = int.Parse(dr["CUS_ID"].ToString()),
                     ShopName = dr["SHOP_NAME"].ToString(),
                     FullName = dr["FULL_NAME"].ToString(),
                     AddressNo = dr["ADDRESS_NO"].ToString(),
                     PhoneNo = dr["PHONE_NO"].ToString(),
                     Postcd = dr["POSTCD"].ToString(),
                     com = _commission,
                     Amount = (int)_amount,
                     Income = (int)_income,
                     FromDate = DateTime.Parse(dr["FROM_DATE"].ToString(), en_US),
                     ToDate = DateTime.Parse(dr["TO_DATE"].ToString(), en_US),
                     //Updated = dr["LASTV"].ToString(),
                  });
               }
               dr.Close();
            }
         }
         return saleModel;
      }
   }
}