using System;

public class SaleSummaryDateDto
{
   public DateTime SaleDate { get; set; }
   public string Employee { get; set; } = "";
   public double Amount { get; set; } = 0;
   public double Discount { get; set; } = 0;
   public double Total { get; set; } = 0;
   public double DifValue { get; set; } = 0;
   public int BillCount { get; set; } = 0;
}