using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MtekApi.Installer;
using MtekApi.Interfaces;

namespace pospos_mobile.Controllers
{
   [Authorize]
   [ApiController]
   [EnableCors("AllowFrontend")]
   [Route("api/[Controller]")]
   public class CustomerController : ControllerBase
   {
      private readonly IAccountService accountService;

      private ServiceResponse<List<CustomerDtos>> CustomerRes;

      private ServiceResponse<int> ResultRes;

      private readonly ICustomerService ICus;

      public CustomerController(ICustomerService ICus, IAccountService accService)
      {
         this.ICus = ICus;
         this.accountService = accService;
         CustomerRes = new ServiceResponse<List<CustomerDtos>>();
         ResultRes = new ServiceResponse<int>();
      }

      [HttpGet("GetCustomer")]
      public async Task<ActionResult> GetCustomer()
      {
         try
         {
            CustomerRes.data = await ICus.GetCustomer();
         }
         catch (Exception ex)
         {
            CustomerRes.IsOk = false;
            CustomerRes.responseMsg = ex.Message;
            return BadRequest(CustomerRes);
         }
         return Ok(CustomerRes);
      }

      [HttpGet("GetCustomerById")]
      public async Task<ActionResult> GetCustomerById(string cusid)
      {
         try
         {
            CustomerRes.data = await ICus.GetCustomerById(cusid);
         }
         catch (Exception ex)
         {
            CustomerRes.IsOk = false;
            CustomerRes.responseMsg = ex.Message;
            return BadRequest(CustomerRes);
         }
         return Ok(CustomerRes);
      }

      [HttpPost("AddNewCustomer")]
      public async Task<ActionResult> GetCustomer([FromBody] CustomerDtos model)
      {
         try
         {
            ResultRes.data = await ICus.AddNewCustomer(model);
         }
         catch (Exception ex)
         {
            ResultRes.responseMsg = ex.Message;
            ResultRes.IsOk = false;
            return BadRequest(ResultRes);
         }
         return StatusCode(201);
      }

      [HttpPost("PostManageCustomer")]
      public async Task<ActionResult> PostManageCustomer([FromBody] CustomerDtos model)
      {
         try
         {
            await ICus.PutManageCustomer(model);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }
         return StatusCode(201);
      }

   }
}