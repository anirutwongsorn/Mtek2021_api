using System;
using mtek_api.Entities;

public class AccountDtos
{
   public int CusId { get; set; }
   public string ShopName { get; set; }
   public bool IsAdmin { get; set; }
   public DateTime expireDate { get; set; }

   public static AccountDtos FromTbUser(TbCustomer model) => new AccountDtos
   {
      CusId = model.CusId,
      ShopName = model.ShopName,
      IsAdmin = (bool)model.Isadmin
   };
}