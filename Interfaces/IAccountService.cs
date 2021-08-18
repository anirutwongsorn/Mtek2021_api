using System.Threading.Tasks;

namespace MtekApi.Installer
{
   public interface IAccountService
   {
      string GenerateToken(AccountDtos account);

      Task Register(RegisterRequestDtos account);

      Task<AccountDtos> Login(string username, string password);

      AccountDtos GetInfo(string accessToken);
   }
}