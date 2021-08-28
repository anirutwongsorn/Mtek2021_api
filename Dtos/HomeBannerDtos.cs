using mtek_api.Entities;

namespace MtekApi.Dtos
{
   public class HomeBannerDtos
   {
      public int id { get; set; }
      public string ImageUrl { get; set; }


      public static HomeBannerDtos FromTbBanner(TbBanner model) => new HomeBannerDtos
      {
         id = model.Id,
         ImageUrl = "images/banner/" + model.ImgUrl
      };

   }
}