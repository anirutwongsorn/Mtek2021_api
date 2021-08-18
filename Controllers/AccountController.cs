using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MtekApi.Installer;

namespace mtek_api.Controllers
{
   [Route("api/[Controller]")]
   public class AccountController : ControllerBase
   {
      private readonly IAccountService accountService;

      public AccountController(IAccountService account)
      {
         this.accountService = account;
      }

      [HttpPost("[action]")]
      public async Task<ActionResult> Register([FromBody] RegisterRequestDtos model)
      {
         await accountService.Register(model);
         return StatusCode((int)HttpStatusCode.Created);
      }

      [HttpPost("[action]")]
      public async Task<ActionResult> Login([FromBody] LoginRequestDtos loginRequest)
      {
         var account = await accountService.Login(loginRequest.Username, loginRequest.Password);
         if (account == null)
         {
            return Unauthorized();
         }
         var model = new MemberDto
         {
            ShopName = account.ShopName,
            // IsAdmin = account.IsAdmin,
            Token = accountService.GenerateToken(account),
         };
         return Ok(new { account = model });
      }

   }
}