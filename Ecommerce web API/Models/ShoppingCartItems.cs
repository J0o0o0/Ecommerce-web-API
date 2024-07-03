using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_web_API.Models
{
    public class ShoppingCartItems
    {
        [ForeignKey("ShoppingCartId")]
        public int ShoppingCartId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public int ammount { get; set; }
    }
}
