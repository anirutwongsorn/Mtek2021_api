using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using mtek_api.Data;
using mtek_api.Entities;
using MtekApi.Installer;
using System.Globalization;

namespace MtekApi.Services
{
   public class AccountService : IAccountService
   {
      private readonly DatabaseContext dbContext;

      private readonly JwtSettings jwtSettings;

      public AccountService(DatabaseContext databaseContext, JwtSettings jwtSettings)
      {
         this.jwtSettings = jwtSettings;
         this.dbContext = databaseContext;
      }

      public async Task<AccountDtos> Login(string username, string password)
      {
         var account = await dbContext.TbCustomers.SingleOrDefaultAsync(a => a.PhoneNo == username);
         if (account != null && VerifyPassword(account.Password, password))
         {
            var _acc = AccountDtos.FromTbUser(account);
            return AccountDtos.FromTbUser(account);
         }
         return null;
      }

      public async Task Register(RegisterRequestDtos account)
      {
         var existingAccount = await dbContext.TbCustomers.SingleOrDefaultAsync(a => a.PhoneNo == account.PhoneNo);
         if (existingAccount != null)
         {
            throw new Exception("The Account is exist!");
         }
         account.Password = CreatePasswordHash(account.Password);

         var cus = new TbCustomer
         {
            ShopName = account.ShopName,
            PhoneNo = account.PhoneNo,
            Password = account.Password,
            AddressNo = account.AddressNo,
            FullName = account.FullName,
            Postcd = account.FullName,
            Isadmin = false,
            Isactv = false,
            Lsactv = DateTime.Now
         };
         dbContext.TbCustomers.Add(cus);
         await dbContext.SaveChangesAsync();
      }

      private string CreatePasswordHash(string password)
      {
         byte[] salt = new byte[128 / 8];
         using (var rng = RandomNumberGenerator.Create())
         {
            rng.GetBytes(salt);
         }

         string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA512,
             iterationCount: 10000,
             numBytesRequested: 258 / 8
         ));
         return $"{Convert.ToBase64String(salt)}.{hashed}";
      }

      private bool VerifyPassword(string hash, string password)
      {
         var parts = hash.Split('.', 2);
         if (parts.Length != 2)
         {
            return false;
         }

         var salt = Convert.FromBase64String(parts[0]);
         var passwordHash = parts[1];
         string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
           password: password,
           salt: salt,
           prf: KeyDerivationPrf.HMACSHA512,
           iterationCount: 10000,
           numBytesRequested: 258 / 8
         ));

         return passwordHash == hashed;
      }

      public string GenerateToken(AccountDtos account)
      {
         // key is case-sensitive
         var en_US = new CultureInfo("en-US");
         var dtString = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss", en_US);
         var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, account.CusId.ToString()),
                new Claim("role", account.ShopName),
                new Claim("permission", account.IsAdmin.ToString()),
                new Claim("expire", dtString),
            };

         return BuildToken(claims);
      }

      public AccountDtos GetInfo(string accessToken)
      {
         var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;

         // key is case-sensitive
         var en_US = new CultureInfo("en-US");
         var userId = token.Claims.First(claim => claim.Type == "sub").Value;
         var role = token.Claims.First(claim => claim.Type == "role").Value;
         var permission = token.Claims.First(claim => claim.Type == "permission").Value;
         var expire = token.Claims.First(claim => claim.Type == "expire").Value;
         var _expireDate = DateTime.Parse(expire, en_US);
         if (_expireDate < DateTime.Now)
         {
            return null;
         }

         var account = new AccountDtos
         {
            CusId = int.Parse(userId),
            ShopName = role,
            IsAdmin = bool.Parse(permission),
            expireDate = _expireDate
         };

         return account;
      }

      private string BuildToken(Claim[] claims)
      {
         var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.Expire));
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         var token = new JwtSecurityToken(
             issuer: jwtSettings.Issuer,
             audience: jwtSettings.Audience,
             claims: claims,
             expires: expires,
             signingCredentials: creds
         );
         return new JwtSecurityTokenHandler().WriteToken(token);
      }

   }

}