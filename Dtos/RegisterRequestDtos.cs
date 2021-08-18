using System.ComponentModel.DataAnnotations;

public class RegisterRequestDtos
{
   [Required]
   public string ShopName { get; set; }

   [Required]
   [Phone]
   public string PhoneNo { get; set; }

   public string AddressNo { get; set; }
   public string FullName { get; set; }
   public string Postcd { get; set; }

   [Required]
   [MinLength(5)]
   public string Password { get; set; }

}
