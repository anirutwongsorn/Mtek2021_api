using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using mtek_api.Data;
using mtek_api.Entities;
using MtekApi.Interfaces;

namespace MtekApi.Services
{
   public class CustomerService : ICustomerService
   {
      private readonly DatabaseContext dbContext;

      public CustomerService(DatabaseContext db)
      {
         this.dbContext = db;
      }

      public async Task<int> AddNewCustomer(CustomerDtos model)
      {
         //===Looking phone=====
         var lookingCus = await dbContext.TbCustomers.FirstOrDefaultAsync(p => p.PhoneNo == model.PhoneNo);
         if (lookingCus != null)
         {
            throw new Exception($"เบอร์มือถือ {model.PhoneNo} เป็นสมาชิกแล้ว");
         }
         var cus = model.Adapt<TbCustomer>();
         cus.Password = CreatePasswordHash(cus.PhoneNo);
         dbContext.TbCustomers.Add(cus);
         return await dbContext.SaveChangesAsync();
      }

      public async Task<List<CustomerDtos>> GetCustomer()
      {
         return (await dbContext.TbCustomers.Where(p => p.Isadmin == false && p.Isactv == false).ToListAsync()).Select(CustomerDtos.FromCusTomer).ToList();
      }

      public async Task<int> GetDefaultCustomer()
      {
         var model = await dbContext.TbCustomers.Where(p => p.Isactv == true).FirstOrDefaultAsync();
         if (model == null)
         {
            throw new Exception("ไม่พบข้อมูลลูกค้า");
         }
         return model.CusId;
      }

      public async Task<TbCusCommission> GetCustomerCommission(int cusid)
      {
         var model = await dbContext.TbCusCommissions.Where(p => p.CusId == cusid).FirstOrDefaultAsync();
         if (model == null)
         {
            throw new Exception("ไม่พบข้อมูลลูกค้า");
         }
         return model;
      }

      public async Task SetCustomerCommission(TbCusCommission model)
      {
         dbContext.TbCusCommissions.Add(model);
         await dbContext.SaveChangesAsync();
      }

      public async Task<List<CustomerDtos>> GetCustomerById(string cusid)
      {
         int id = 0;
         int.TryParse(cusid, out id);
         return (await dbContext.TbCustomers.Where(p => p.CusId == id).ToListAsync()).Select(CustomerDtos.FromCusTomer).ToList();
      }

      public async Task PutManageCustomer(CustomerDtos model)
      {
         var result = await dbContext.TbCustomers.SingleOrDefaultAsync(p => p.CusId == model.CusId);
         if (result == null)
         {
            throw new System.Exception("Member is not existing");
         }
         result.AddressNo = model.AddressNo;
         result.FullName = model.FullName;
         result.ShopName = model.ShopName;
         result.Postcd = model.Postcd;
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

   }

}