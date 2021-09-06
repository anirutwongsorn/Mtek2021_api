using System.Collections.Generic;
using System.Threading.Tasks;
using mtek_api.Entities;

namespace MtekApi.Interfaces
{
   public interface ICustomerService
   {
      Task<List<CustomerDtos>> GetCustomer();

      Task<int> GetDefaultCustomer();

      Task<List<CustomerDtos>> GetCustomerById(string cusid);

      Task<int> AddNewCustomer(CustomerDtos model);

      Task PutManageCustomer(CustomerDtos model);

      Task<TbCusCommission> GetCustomerCommission(int cusid);

      Task SetCustomerCommission(TbCusCommission model);
   }
}