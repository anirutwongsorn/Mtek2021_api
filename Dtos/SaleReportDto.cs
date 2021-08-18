using System;
using System.Collections.Generic;
using System.Linq;
using mtek_api.Entities;

public class SaleReportDto
{
   public string BillCd { get; set; }
   public string Cusname { get; set; }
   public string Emp { get; set; }
   public decimal GAmt { get; set; }
   public decimal GDiscnt { get; set; }
   public decimal GTotal { get; set; }
   public DateTime Actv { get; set; }

   public List<SaleReportProductDto> ProductSale { get; set; }

   public static SaleReportDto FromBillHeader(TbBillHeader model)
   {
      return new SaleReportDto
      {
         BillCd = model.Billcd,
         Cusname = model.Cus.ShopName,
         Emp = model.Emp,
         GAmt = (decimal)model.GAmt,
         GDiscnt = (decimal)model.GDiscnt,
         GTotal = (decimal)model.GTotal,
         Actv = (DateTime)model.Actv,
         ProductSale = model.TbBillWos.Select(SaleReportProductDto.FromBillWo).ToList(),
      };
   }
}

public class SaleReportProductDto
{
   public string Pcd { get; set; }
   public string Pdesc { get; set; }
   public string Uom { get; set; }
   public decimal Qty { get; set; }
   public decimal Prcs { get; set; }
   public decimal Discount { get; set; }
   public decimal Amt { get; set; }

   public static SaleReportProductDto FromBillWo(TbBillWo model)
   {
      return new SaleReportProductDto
      {
         Pcd = model.Pcd,
         Pdesc = model.PcdNavigation.Pdesc,
         Uom = model.Uom,
         Qty = (decimal)model.Qty,
         Prcs = (decimal)model.Prcs,
         Discount = (decimal)model.Discount,
         Amt = (decimal)model.Amt,
      };
   }
}
