using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MtekApi.Dtos;
using MtekApi.Installer;
using MtekApi.Interfaces;

namespace MtekApi.Controllers
{
   [Authorize]
   [ApiController]
   [EnableCors("AllowFrontend")]
   [Route("api/[Controller]")]
   public class PosBillController : ControllerBase
   {
      private readonly IAccountService accountService;
      private ServiceResponse<List<PosBillDto>> BillPosRes;
      private ServiceResponse<List<SaleReportDto>> BillReportRes;
      private ServiceResponse<List<ProductCusDto>> BillMainRes;
      private readonly IBillReportService billService;

      public PosBillController(IBillReportService billService, IAccountService accService)
      {
         this.accountService = accService;
         this.billService = billService;
         BillPosRes = new ServiceResponse<List<PosBillDto>>();
         BillReportRes = new ServiceResponse<List<SaleReportDto>>();
         BillMainRes = new ServiceResponse<List<ProductCusDto>>();
      }

      [HttpGet("GetSaleReport")]
      public async Task<ActionResult> GetSaleReport()
      {
         try
         {
            BillPosRes.data = await billService.GetAllBillMain();
         }
         catch (Exception ex)
         {
            BillPosRes.IsOk = false;
            BillPosRes.responseMsg = ex.InnerException.Message.ToString();
         }
         return Ok(BillPosRes);
      }

      [HttpGet("GetSaleReportDetails")]
      public async Task<ActionResult> GetSaleReportDetails(string billCd)
      {
         try
         {
            if (billCd == null)
            {
               throw new Exception("กรุณาระบุเลขที่อ้างอิง");
            }

            BillPosRes.data = await billService.GetAllBillMainDetails(billCd);
         }
         catch (Exception ex)
         {
            BillPosRes.IsOk = false;
            BillPosRes.responseMsg = ex.InnerException.Message.ToString();
         }
         return Ok(BillPosRes);
      }

      [HttpGet("GetSaleReportByCus")]
      public async Task<ActionResult> GetSaleReportDetails(DateTime dFrom, DateTime dTo, int cusid)
      {
         try
         {
            BillReportRes.data = await billService.GetSaleReportByCus(dFrom, dTo, cusid);
         }
         catch (Exception ex)
         {
            BillReportRes.IsOk = false;
            BillReportRes.responseMsg = ex.Message;
            return BadRequest(BillReportRes);
         }
         return Ok(BillReportRes);

      }

      [HttpGet("GetSaleReportByCusid")]
      public async Task<ActionResult> GetSaleReportByCusid()
      {
         try
         {
            var acc = await GetUserInfo();
            BillMainRes.data = await billService.GetSaleBillMainByCusid(acc.CusId);
         }
         catch (Exception ex)
         {
            BillMainRes.IsOk = false;
            BillMainRes.responseMsg = ex.Message;
            return BadRequest(BillMainRes);
         }
         return Ok(BillMainRes);

      }

      private async Task<AccountDtos> GetUserInfo()
      {
         var accessToken = await HttpContext.GetTokenAsync("access_token");
         if (accessToken == null)
         {
            return null;
         }

         return accountService.GetInfo(accessToken);
      }

   }
}