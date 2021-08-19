using System;

namespace MtekApi.Dtos
{
   public class PosBillDto
   {
      public string BillCd { get; set; }
      public int CusId { get; set; }
      // public string CrdNo { get; set; }
      public string FullName { get; set; }
      // public string LastName { get; set; }
      public string ShopName { get; set; }
      public string PhoneNo { get; set; }
      //public string Building { get; set; }
      public string AddressNo { get; set; }
      public string Emp { get; set; } = "admin";
      public decimal GAmnt { get; set; } = 0;
      public decimal GDiscnt { get; set; } = 0;
      public decimal Vat { get; set; } = 0;
      public decimal GTotal { get; set; } = 0;
      public string Remark { get; set; }

      public string Pcd { get; set; }
      public string Pdesc { get; set; }
      public string Uom { get; set; }
      public decimal Qty { get; set; } = 0;
      public decimal Prcs { get; set; } = 0;
      public decimal Amount { get; set; } = 0;
      public string ImgPath { get; set; } = "";
      public DateTime Actv { get; set; }
   }
}