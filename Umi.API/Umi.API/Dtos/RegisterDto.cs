using System.ComponentModel.DataAnnotations;

namespace Umi.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public  string Email { get; set; }
        
        [Required]
        public  string Password { get; set; }
        
        [Required]
        [Compare(nameof(Password), ErrorMessage =  "inconsistent password")]
        public  string ConfirmPassword { get; set; }
    }
}