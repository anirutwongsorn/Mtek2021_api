using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


public class ProductRequestDtos
{
   public string Pcd { get; set; }
   public string Pdesc { get; set; }
   public int GpCd { get; set; }
   public string Uom { get; set; }
   public string ImgPath { get; set; }
   public decimal Blqty { get; set; } = 0;
   public decimal Stock { get; set; } = 0;
   public decimal PrcCost { get; set; } = 0;
   public decimal PrcSale { get; set; } = 0;
   public List<IFormFile> FormFiles { get; set; }
}
