using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MtekApi.Dtos;
using MtekApi.Interfaces;

namespace MtekApi.Controllers
{
   [Authorize]
   [ApiController]
   [EnableCors("AllowFrontend")]
   [Route("api/[Controller]")]
   public class PosBillController : ControllerBase
   {
      private ServiceResponse<List<PosBillDto>> BillPosRes;
      private readonly IBillReportService billService;

      public PosBillController(IBillReportService billService)
      {
         this.billService = billService;
         BillPosRes = new ServiceResponse<List<PosBillDto>>();
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

   }
}