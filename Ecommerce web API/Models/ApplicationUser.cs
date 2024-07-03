using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_web_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

        public decimal? Balance { get; set; } = 0;
        

    }
}
