using System.Collections.Generic;
using System.Threading.Tasks;
using MtekApi.Dtos;

namespace MtekApi.Interfaces
{
   public interface IBillReportService
   {
      Task<List<PosBillDto>> GetAllBillMain();

      Task<List<PosBillDto>> GetAllBillMainDetails(string billCd);
   }
}