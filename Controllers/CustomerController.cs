using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
      
      private readonly ICustomerService ICus;

      private readonly IProductService productService;

      private ServiceResponse<List<CustomerDtos>> CustomerRes;

      private ServiceResponse<List<ProductCusDto>> ProductCusRes;

      private ServiceResponse<int> ResultRes;

      public CustomerController(ICustomerService ICus, IAccountService accService, IProductService productService)
      {
         this.ICus = ICus;
         this.accountService = accService;
         this.productService = productService;

         CustomerRes = new ServiceResponse<List<CustomerDtos>>();
         ProductCusRes = new ServiceResponse<List<ProductCusDto>>();
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

      [HttpGet("GetMemberInfoById")]
      public async Task<ActionResult> GetAccountInfoById()
      {
         try
         {
            var cus = await GetUserInfo();
            CustomerRes.data = await ICus.GetCustomerById(cus.CusId.ToString());
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

      //=================Product stock=====================
      [HttpGet("GetProductInventoryByCusId")]
      public async Task<ActionResult> GetProductInventoryByCusId()
      {
         try
         {
            var cus = await GetUserInfo();
            var cusid = cus.CusId;
            ProductCusRes.data = await productService.GetProductInventoryByCusId(cusid);
         }
         catch (Exception ex)
         {
            ProductCusRes.IsOk = false;
            ProductCusRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductCusRes);
         }
         return Ok(ProductCusRes);
      }

      [HttpGet("GetProductTransByCusByProduct")]
      public async Task<ActionResult> GetProductTransByCus(string pcd)
      {
         var account = await GetUserInfo();

         if (pcd != null)
         {
            try
            {
               var model = await productService.GetProductTransByCus(account.CusId, pcd);
               ProductCusRes.data = model;
            }
            catch (Exception ex)
            {
               ProductCusRes.IsOk = false;
               ProductCusRes.responseMsg = ex.Message.ToString();
               return BadRequest(ProductCusRes);
            }
         }

         return Ok(ProductCusRes);
      }

      //=================Product stock=====================
      [HttpGet("GetProductInventoryByCusIdByAdmin")]
      public async Task<ActionResult> GetProductInventoryByCusIdByAdmin(int cusid)
      {
         try
         {
            var cus = await GetUserInfo();
            if (!cus.IsAdmin)
            {
               return Unauthorized();
            }

            ProductCusRes.data = await productService.GetProductInventoryByCusId(cusid);
         }
         catch (Exception ex)
         {
            ProductCusRes.IsOk = false;
            ProductCusRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductCusRes);
         }
         return Ok(ProductCusRes);
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