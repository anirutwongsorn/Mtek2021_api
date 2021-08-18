using System;
using System.Collections.Generic;
using System.Linq;
using mtek_api.Entities;

public class SaleReportDescDtos
{
   public string BillCd { get; set; }
   public int CusId { get; set; }
   public string Cusname { get; set; } = "ลูกค้าทั่วไป";
   public string Emp { get; set; } = "admin";
   public decimal GAmnt { get; set; } = 0;
   public decimal GDiscnt { get; set; } = 0;
   public decimal Vat { get; set; } = 0;
   public decimal GTotal { get; set; } = 0;
   public DateTime Actv { get; set; }

   public List<SaleReportProductDto> ProductSale { get; set; }

   public static SaleReportDescDtos FromTbBillMain(TbBillHeader model)
   {
      return new SaleReportDescDtos
      {
         BillCd = model.Billcd,
         Cusname = model.Cus.ShopName,
         Emp = model.Emp,
         GAmnt = (decimal)model.GAmt,
         GDiscnt = (decimal)model.GDiscnt,
         Vat = (decimal)model.Vat,
         GTotal = (decimal)model.GTotal,
         Actv = (DateTime)model.Actv,
         ProductSale = model.TbBillWos.Select(SaleReportProductDto.FromBillWo).ToList(),
      };
   }

}
