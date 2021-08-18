
public class PosSaleDto
{
   public string billtype { get; set; }
   public string BillCd { get; set; }
   public int CusId { get; set; }
   // public string Idcard { get; set; } = "0";
   // public string Cusname { get; set; } = "ลูกค้าทั่วไป";
   // public string Phoneno { get; set; } = "";
   public string Emp { get; set; } = "admin";
   public decimal GAmnt { get; set; } = 0;
   public decimal GDiscnt { get; set; } = 0;
   public decimal Vat { get; set; } = 0;
   public decimal GTotal { get; set; } = 0;
   public string Pcd { get; set; }
   // public string Pdesc { get; set; }
   public string Uom { get; set; }
   public decimal Qty { get; set; } = 0;
   public decimal Prcs { get; set; } = 0;
   public decimal Discount { get; set; } = 0;
   public decimal Amount { get; set; } = 0;
   //public string ImgPath { get; set; }

}
