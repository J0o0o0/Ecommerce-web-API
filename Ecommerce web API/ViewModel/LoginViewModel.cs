using System.ComponentModel.DataAnnotations;

namespace Ecommerce_web_API.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememperMe { get; set; }
    }
}
