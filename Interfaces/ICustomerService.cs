using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtekApi.Interfaces
{
   public interface ICustomerService
   {
      Task<List<CustomerDtos>> GetCustomer();

      Task<List<CustomerDtos>> GetCustomerById(string cusid);

      Task<int> AddNewCustomer(CustomerDtos model);

      Task PutManageCustomer(CustomerDtos model);
   }
}