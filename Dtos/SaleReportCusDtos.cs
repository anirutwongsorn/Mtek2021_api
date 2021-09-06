using System;

namespace MtekApi.Dtos
{
   public class SaleReportCusDtos
   {
      public int CusId { get; set; } = 0;
      public string FullName { get; set; } = "";
      public string ShopName { get; set; } = "";
      public string PhoneNo { get; set; } = "";
      public string AddressNo { get; set; } = "";
      public string Postcd { get; set; } = "";
      public DateTime FromDate { get; set; }
      public DateTime ToDate { get; set; }
      public int com { get; set; } = 0;
      public int Amount { get; set; } = 0;
      public int Income { get; set; } = 0;
   }
}