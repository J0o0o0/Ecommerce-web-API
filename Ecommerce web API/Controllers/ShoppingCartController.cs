using Ecommerce_web_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce_web_API.Controllers
{
    [Authorize(Roles ="Buyer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
       // [Authorize("Buyer")]
        public IActionResult Get()
        { 

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (context.ShoppingCart.Where(y => y.UserId == UserId).ToList().Count() > 0)
            {
                //var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);

            }
            else
            {
                //if (Cart == null)
                {
                    ShoppingCart cart = new ShoppingCart();
                    cart.UserId = UserId;
                    context.ShoppingCart.Add(cart);
                    context.SaveChanges();
                }

            }
            var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);

            var CartId = Cart.Id;

            var Items = context.ShoppingCartItems.Where(x => x.ShoppingCartId == CartId).ToList();

           List<Product> Products = new List<Product>();
            foreach (var item in Items)
            {
                Product product = context.Products.Single(x => x.Id == item.ProductId);

                
              Products.Add(product);
            }
            UpdateTotal();

            return Ok(Products);
        }

        [HttpPost]
       // [Authorize("Buyer")]
        public IActionResult Post (int ProductId , int ammount)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (context.ShoppingCart.Where(y => y.UserId == UserId).ToList().Count() > 0)
            {
               

            }
            else
            {
                
                {
                    ShoppingCart cart = new ShoppingCart();
                    cart.UserId = UserId;
                    context.ShoppingCart.Add(cart);
                    context.SaveChanges();
                }

            }

            
            var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);

            var CartId = Cart.Id;

            ShoppingCartItems found = context.ShoppingCartItems.SingleOrDefault(x => x.ProductId == ProductId && x.ShoppingCartId == CartId);
            if (found != null)
            {
                found.ammount += ammount;
                context.Entry(found).State = EntityState.Modified ;
                context.SaveChanges();
            }
            else
            {

                ShoppingCartItems newItem = new ShoppingCartItems();
                newItem.ProductId = ProductId;
                newItem.ShoppingCartId = CartId;
                newItem.ammount = ammount;
                context.ShoppingCartItems.Add(newItem);
                context.SaveChanges();

            }





            
            UpdateTotal();

            return Ok();
        }

        [HttpDelete]

        public IActionResult Delete( int productId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);
            var shopingCartId = Cart.Id;

            if (context.ShoppingCartItems == null)
            {
                return NotFound();
            }
            var product = context.ShoppingCartItems.Find(shopingCartId,productId);
            if (product == null)
            {
                return NotFound();
            }

            context.ShoppingCartItems.Remove(product);
            context.SaveChanges();



            UpdateTotal();

            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateTotal() 
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);
            var CartId = Cart.Id;
            var Items = context.ShoppingCartItems.Where(x => x.ShoppingCartId == CartId).ToList();
            decimal Total = 0;
            foreach (var item in Items)
            {
                Product product = context.Products.Single(x => x.Id == item.ProductId);

                var tmp = product.Price * item.ammount;
                Total += tmp;

            }
            Cart.Total = Total;

            context.Entry(Cart).State = EntityState.Modified;
            context.SaveChanges();

            return NoContent();
        }
        

    }
}
