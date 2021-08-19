using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MtekApi.Interfaces
{
   public interface IProductService
   {
      Task<List<ProductDto>> GetLocalProduct();

      Task<List<ProductCusDto>> GetProductBranch(int cuscd, string pcd = "");

      Task<List<ProductCusDto>> GetAllProductBranch();

      Task<List<ProductDto>> GetProductById(string pcd);

      Task<List<ProductCusDto>> GetProductTrans(string pcd);

      Task<int> PostAddNewProduct(ProductRequestDtos model);

      Task<int> PutManageProduct(ProductRequestDtos model);

      // Task<int> DeleteProduct(string pcd);

      // Task<int> ReleaseProductToSale(string pcd);

      // Task<int> ReturnToNormal(string pcd = "All");

      Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles);

      Task<List<ProductGroupDtos>> GetProductGroup();
   }
}