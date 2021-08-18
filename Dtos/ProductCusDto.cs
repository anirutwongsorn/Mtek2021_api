using System;

  public class ProductCusDto
  {
    public int CusId { get; set; }
    public string FullName { get; set; }
    public string ShopName { get; set; }
    public string PhoneNo { get; set; }
    public string Postcd { get; set; }
    public string Pcd { get; set; }
    public string Pdesc { get; set; }
    public string Gpcd { get; set; }
    public string Gpdesc { get; set; }
    public string Uom { get; set; }
    public string ImgPath { get; set; }
    public int Stock { get; set; } = 0;
    public int Blqty { get; set; } = 0;
    public DateTime Lsactv { get; set; } = DateTime.Now;
  }
