using System;
using System.Collections.Generic;
using System.Linq;
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
   public class ProductController : ControllerBase
   {
      private readonly IAccountService accountService;
      private ServiceResponse<List<ProductDto>> ProductRes;
      private ServiceResponse<List<ProductCusDto>> ProductCusRes;
      private ServiceResponse<List<ProductGroupDtos>> ProductGroupRes;

      private ServiceResponse<int> ResultRes;

      private readonly IProductService productService;

      public ProductController(IProductService productService, IAccountService accService)
      {
         this.productService = productService;
         this.accountService = accService;
         ProductRes = new ServiceResponse<List<ProductDto>>();
         ProductCusRes = new ServiceResponse<List<ProductCusDto>>();
         ProductGroupRes = new ServiceResponse<List<ProductGroupDtos>>();
         ResultRes = new ServiceResponse<int>();
      }

      [HttpGet("GetLocalProduct")]
      public async Task<ActionResult> GetLocalProduct()
      {
         var acc = await GetUserInfo();
         if (!acc.IsAdmin)
         {
            return Unauthorized();
         }

         try
         {
            var model = await productService.GetLocalProduct();
            ProductRes.data = model.OrderBy(p => p.Pdesc).ToList();
         }
         catch (Exception ex)
         {
            ProductRes.IsOk = false;
            ProductRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductRes);
         }
         return Ok(ProductRes);
      }

      [HttpGet("GetProductById")]
      public async Task<ActionResult> GetProductById(string pcd)
      {
         try
         {
            if (pcd == null)
            {
               throw new Exception("กรุณาระบุรหัสสินค้า");
            }

            ProductRes.data = await productService.GetProductById(pcd);
         }
         catch (Exception ex)
         {
            ProductRes.IsOk = false;
            ProductRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductRes);
         }
         return Ok(ProductRes);
      }

      [HttpGet("GetProductBranch")]
      public async Task<ActionResult> GetProductBranch()
      {
         var account = await GetUserInfo();
         try
         {
            var model = await productService.GetProductBranch(account.CusId);
            ProductCusRes.data = model;
         }
         catch (Exception ex)
         {
            ProductCusRes.IsOk = false;
            ProductCusRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductCusRes);
         }
         return Ok(ProductCusRes);
      }

      [HttpGet("GetAllProductBranch")]
      public async Task<ActionResult> GetAllProductBranch(string pcd = "")
      {
         var account = await GetUserInfo();
         if (!account.IsAdmin)
         {
            return Unauthorized();
         }

         try
         {
            var model = await productService.GetAllProductBranch();
            if (pcd == null || pcd == "")
            {
               ProductCusRes.data = model;
            }
            else
            {
               ProductCusRes.data = model.Where(p => p.Pcd == pcd).ToList();
            }
         }
         catch (Exception ex)
         {
            ProductCusRes.IsOk = false;
            ProductCusRes.responseMsg = ex.Message.ToString();
            return BadRequest(ProductCusRes);
         }
         return Ok(ProductCusRes);
      }

      [HttpGet("GetProductTrans")]
      public async Task<ActionResult> GetProductTrans(string pcd)
      {
         var account = await GetUserInfo();
         if (!account.IsAdmin)
         {
            return Unauthorized();
         }

         if (pcd != null)
         {
            try
            {
               var model = await productService.GetProductTrans(pcd);
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

      [HttpPost("PostAddNewProduct")]
      public async Task<ActionResult> PostAddNewProduct([FromForm] ProductRequestDtos model)
      {
         (string errorMessage, string imageName) = await productService.UploadImage(model.FormFiles);
         if (!String.IsNullOrEmpty(errorMessage))
         {
            return BadRequest();
         }

         model.ImgPath = imageName;
         try
         {
            ResultRes.data = await productService.PostAddNewProduct(model);
         }
         catch (Exception ex)
         {
            ResultRes.IsOk = false;
            ResultRes.responseMsg = ex.Message;
            return BadRequest(ResultRes);
         }
         return StatusCode(201);
      }

      [HttpPut("PutManageProduct")]
      public async Task<ActionResult> PutManageProduct([FromForm] ProductRequestDtos model)
      {
         (string errorMessage, string imageName) = await productService.UploadImage(model.FormFiles);
         if (!String.IsNullOrEmpty(errorMessage))
         {
            return BadRequest();
         }

         model.ImgPath = imageName;
         try
         {
            ResultRes.data = await productService.PutManageProduct(model);
         }
         catch (Exception ex)
         {
            ResultRes.IsOk = false;
            ResultRes.responseMsg = ex.Message;
            return BadRequest(ResultRes);
         }
         return StatusCode(201);
      }


      [HttpGet("GetProductGroup")]
      public async Task<ActionResult> GetProductGroup()
      {
         try
         {
            ProductGroupRes.data = await productService.GetProductGroup();
         }
         catch (Exception ex)
         {
            ProductGroupRes.IsOk = false;
            ProductGroupRes.responseMsg = ex.InnerException.Message;
            return BadRequest(ProductGroupRes);
         }
         return Ok(ProductGroupRes);
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