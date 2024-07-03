using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_web_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int  Amount { get; set; }

        [ForeignKey("SellerId")]
        public string SellerId { get; set; }
        public string UserName { get; set; }

    }
}
