using System.ComponentModel.DataAnnotations;

public class LoginRequestDtos
{
   [Required]
   public string Username { get; set; }

   [Required]
   [MinLength(5)]
   public string Password { get; set; }
}