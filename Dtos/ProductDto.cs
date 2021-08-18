using System;
using System.Collections.Generic;
using System.Globalization;
using mtek_api.Entities;

public class ProductDto : ProductGroupDtos
{
   public string Pcd { get; set; }
   public string Pdesc { get; set; }
   public string Uom { get; set; }
   public decimal PrcCost { get; set; } = 0;
   public decimal PrcSale { get; set; } = 0;
   public int Minstk { get; set; }
   public string ImgPath { get; set; }
   public decimal Stock { get; set; } = 0;
   public decimal Blqty { get; set; } = 0;
   public DateTime Lsactv { get; set; }

   public static ProductDto FromTbProduct(TbProduct product)
   {
      return new ProductDto
      {
         Pcd = product.Pcd,
         Pdesc = product.Pdesc,
         Gpcd = (int)product.Gpcd,
         Gpdesc = product.GpcdNavigation.Gpdesc,
         Uom = product.Uom,
         ImgPath = product.ImgPath,
         PrcCost = (decimal)product.PrcCost,
         PrcSale = (decimal)product.PrcSale,
         Blqty = 0,
         Stock = (int)product.Stock,
         Lsactv = (DateTime)product.Lsactv,
      };
   }

}
