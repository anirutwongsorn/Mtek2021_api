
using mtek_api.Entities;

public class ProductGroupDtos
{
   public int Gpcd { get; set; }
   public string Gpdesc { get; set; }

   public static ProductGroupDtos FromTbProductGroup(TbProductGroup model) => new ProductGroupDtos
   {
      Gpcd = model.Gpcd,
      Gpdesc = model.Gpdesc
   };
}