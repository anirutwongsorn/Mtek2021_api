using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MtekApi.Dtos;

namespace MtekApi.Interfaces
{
   public interface IProductService
   {
      Task<List<ProductDto>> GetLocalProduct();

      Task<List<ProductCusDto>> GetProductBranch(int cuscd, string pcd = "");

      Task<List<ProductCusDto>> GetAllProductBranch();

      Task<List<ProductDto>> GetProductById(string pcd);

      Task<List<ProductCusDto>> GetProductInventoryByCusId(int cusid);

      Task<List<ProductCusDto>> GetProductTrans(string pcd);

      Task<List<ProductCusDto>> GetProductTransByCus(int cusid, string pcd);

      Task<int> PostAddNewProduct(ProductRequestDtos model);

      Task<int> PutManageProduct(ProductRequestDtos model);

      Task<List<HomeBannerDtos>> GetHomeBanner();

      Task<(string errorMessage, string imageName)> UploadImage(List<IFormFile> formFiles);

      Task<List<ProductGroupDtos>> GetProductGroup();
   }
}