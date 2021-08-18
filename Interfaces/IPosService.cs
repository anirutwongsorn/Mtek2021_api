using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtekApi.Interfaces
{
   public interface IPosService
   {
      Task<int> PostSale(List<PosSaleDto> productSale);

      Task<List<SaleReportDto>> GetSaleReport(DateTime dateFrom, DateTime dateTo);

      Task<List<SaleReportDto>> GetSaleReportByCus(int cusid, string pcd);

      Task<List<SaleSummaryDateDto>> GetSaleSummaryByDate();

      Task<List<SaleSummaryDateDto>> GetSaleSummaryByDateByEmployee(DateTime dateFrom, DateTime dateTo);

      Task<List<SaleReportDescDtos>> GetSaleSummaryByBillCd(string billCd);

      Task<List<SaleReportDescDtos>> GetSaleSummaryByBillCd(DateTime dateFrom, DateTime dateTo);

   }
}