
using mtek_api.Entities;

public class CustomerDtos
{
   public int CusId { get; set; }
   // public string CrdNo { get; set; }
   public string FullName { get; set; }
   // public string LastName { get; set; }
   public string ShopName { get; set; }
   public string PhoneNo { get; set; }
   //public string Building { get; set; }
   public string AddressNo { get; set; }
   //public string Moo { get; set; }
   //public string Road { get; set; }
   //public string Village { get; set; }
   //public string Province { get; set; }
   //public string District { get; set; }
   //public string Subdistinct { get; set; }
   public string Postcd { get; set; }

   public static CustomerDtos FromCusTomer(TbCustomer cus)
   {
      return new CustomerDtos
      {
         CusId = cus.CusId,
         FullName = cus.FullName,
         ShopName = cus.ShopName,
         PhoneNo = cus.PhoneNo,
         AddressNo = cus.AddressNo,
         Postcd = cus.Postcd,
      };
   }

}
