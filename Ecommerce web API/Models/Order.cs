using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_web_API.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
    }
}
