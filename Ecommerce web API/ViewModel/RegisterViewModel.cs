using System.ComponentModel.DataAnnotations;

namespace Ecommerce_web_API.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string Role { get; set; }

    }
    public enum  Role
    {
        Seller,Buyer
    }
}
