using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using MtekApi.Installer;
using MtekApi.Interfaces;

namespace pospos_mobile.Controllers
{
   [Authorize]
   [ApiController]
   [EnableCors("AllowFrontend")]
   [Route("api/[Controller]")]
   public class PosController : ControllerBase
   {
      private readonly IAccountService accountService;

      private ServiceResponse<int> PosRes;

      private ServiceResponse<List<SaleReportDto>> SaleReportRes;

      private ServiceResponse<List<SaleReportDescDtos>> SaleReportDescRes;

      private ServiceResponse<List<SaleSummaryDateDto>> SaleSummaryDateRes;

      private readonly IPosService posService;

      public PosController(IPosService posService, IAccountService accService)
      {
         this.posService = posService;
         this.accountService = accService;
         PosRes = new ServiceResponse<int>();
         SaleReportRes = new ServiceResponse<List<SaleReportDto>>();
         SaleSummaryDateRes = new ServiceResponse<List<SaleSummaryDateDto>>();
         SaleReportDescRes = new ServiceResponse<List<SaleReportDescDtos>>();
      }

      [HttpPost("PostPosSale")]
      public async Task<ActionResult> PostPos([FromBody] List<PosSaleDto> model)
      {
         var _account = await GetUserInfo();
         if (model[0].billtype.Contains("TG") && _account.IsAdmin == false)
         {
            return Unauthorized();
         }

         try
         {
            int resp = await posService.PostSale(model);
            PosRes.data = resp;
            if (resp <= 0)
            {
               throw new Exception("Save Error");
            }
         }
         catch (Exception ex)
         {
            PosRes.data = -1;
            PosRes.IsOk = false;
            PosRes.responseMsg = ex.InnerException.Message.ToString();
            return BadRequest(PosRes);
         }
         return StatusCode(201);
      }

      [HttpGet("GetSaleReport")]
      public async Task<ActionResult> GetSaleReport(DateTime dateFrom, DateTime dateTo)
      {
         try
         {
            if (dateFrom == new DateTime())
            {
               dateFrom = DateTime.Now.Date;
               dateTo = DateTime.Now.Date;
            }
            SaleReportRes.data = await posService.GetSaleReport(dateFrom, dateTo);
         }
         catch (Exception ex)
         {
            SaleReportRes.IsOk = false;
            SaleReportRes.responseMsg = ex.InnerException.Message.ToString();
         }
         return Ok(SaleReportRes);
      }

      [HttpGet("GetSaleReportByCustomerByProductCode")]
      public async Task<ActionResult> GetSaleReportByCustomerByProductCode(int? cuscd, string pcd)
      {
         if (cuscd == null || pcd == null)
         {
            return BadRequest("กรุณาระบุเงื่อนไขการค้นหา !");
         }

         try
         {
            SaleReportRes.data = await posService.GetSaleReportByCus((int)cuscd, pcd);
         }
         catch (Exception ex)
         {
            SaleReportRes.IsOk = false;
            SaleReportRes.responseMsg = ex.InnerException.Message.ToString();
         }
         return Ok(SaleReportRes);
      }

      [HttpGet("GetSaleSummaryByDate")]
      public async Task<ActionResult> GetSaleSummaryDate()
      {
         try
         {
            SaleSummaryDateRes.data = await posService.GetSaleSummaryByDate();
         }
         catch (Exception ex)
         {
            SaleSummaryDateRes.IsOk = false;
            SaleSummaryDateRes.responseMsg = ex.Message.ToString();
         }
         return Ok(SaleSummaryDateRes);
      }

      [HttpGet("GetSaleSummaryByDateByEmployee")]
      public async Task<ActionResult> GetSaleSummaryByDateByEmployee(DateTime dateFrom, DateTime dateTo)
      {
         try
         {
            var _model = await posService.GetSaleSummaryByDateByEmployee(dateFrom, dateTo);
            SaleSummaryDateRes.data = _model;
         }
         catch (Exception ex)
         {
            SaleSummaryDateRes.IsOk = false;
            SaleSummaryDateRes.responseMsg = ex.Message.ToString();
         }
         return Ok(SaleSummaryDateRes);
      }

      [HttpGet("GetSaleSummaryByBillCd")]
      public async Task<ActionResult> GetSaleSummaryByBillCd(string billCd)
      {
         try
         {
            SaleReportDescRes.data = await posService.GetSaleSummaryByBillCd(billCd);
         }
         catch (Exception ex)
         {
            SaleReportDescRes.IsOk = false;
            SaleReportDescRes.responseMsg = ex.Message;
            return BadRequest(SaleReportDescRes);
         }
         return Ok(SaleReportDescRes);
      }

      [HttpGet("GetSaleSummaryByDescDate")]
      public async Task<ActionResult> GetSaleSummaryByDescDate(DateTime dateFrom, DateTime dateTo)
      {
         try
         {
            SaleReportDescRes.data = await posService.GetSaleSummaryByBillCd(dateFrom, dateTo);
         }
         catch (Exception ex)
         {
            SaleReportDescRes.IsOk = false;
            SaleReportDescRes.responseMsg = ex.Message;
            return BadRequest(SaleReportDescRes);
         }
         return Ok(SaleReportDescRes);
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