using Ecommerce_web_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Ecommerce_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OrderController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet]

        public IActionResult Get()
        {
            return Ok();
        }
        

        [HttpPost]
        public IActionResult NewOrder(string address)
        {

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId == null) { return (BadRequest()); }

            var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);
            var CartId = Cart.Id;
            var Items = context.ShoppingCartItems.Where(x => x.ShoppingCartId == CartId).ToList();
            if(Items.Count == 0) 
            {
                return NotFound();
            }
            else
            {
                

                
                if (RemoveBalance(Cart.Total))
                {
                    Order NewOrder = new Order();
                    NewOrder.Address = address;
                    NewOrder.UserId = UserId;
                    NewOrder.Total = Cart.Total;


                    context.Orders.Add(NewOrder);
                    context.SaveChanges();
                    

                    List<Product> Products = new List<Product>();
                foreach (var item in Items)
                {
                    Product product = context.Products.Single(x => x.Id == item.ProductId);

                    OrderItems orderItem = new OrderItems();

                    orderItem.OrderId = NewOrder.Id;
                    orderItem.ProductId = item.ProductId;
                    orderItem.ProductCount = item.ammount;

                    context.OrderItems.Add(orderItem);
                    context.SaveChanges();

                    context.ShoppingCartItems.Remove(item);
                    context.SaveChanges();

                    product.Amount = product.Amount - item.ammount;

                        context.Entry(product).State = EntityState.Modified;
                        context.SaveChanges();

                    ApplicationUser user = context.applicationUsers.Single(x => x.Id.Equals(product.SellerId));
                        AddBalance(user.Id, product.Price * item.ammount);

                        Products.Add(product);
                }

                    return Ok(Products);

                }else
                {
                    
                    return BadRequest();
                }

            }

        }

        // [HttpDelete]

        //public IActionResult CancelOrder()
        //{
        //    var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var Cart = context.ShoppingCart.Single(y => y.UserId == UserId);
        //    var CartId = Cart.Id;
        //    var Items = context.ShoppingCartItems.Where(x => x.ShoppingCartId == CartId).ToList();
        //    if (Items.Count == 0)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Order NewOrder = new Order();
        //        NewOrder.Address = address;
        //        NewOrder.UserId = UserId;
        //        NewOrder.Total = Cart.Total;

        //        context.Orders.Add(NewOrder);
        //        context.SaveChanges();



        //        List<Product> Products = new List<Product>();
        //        foreach (var item in Items)
        //        {
        //            Product product = context.Products.Single(x => x.Id == item.ProductId);

        //            OrderItems orderItem = new OrderItems();

        //            orderItem.OrderId = NewOrder.Id;
        //            orderItem.ProductId = item.ProductId;

        //            context.OrderItems.Add(orderItem);
        //            context.SaveChanges();

        //            Products.Add(product);
        //        }

        //        RemoveBalance(NewOrder.Total);

        //        return Ok(Products);

        //    }

        //}



        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult AddBalance(string UserId, decimal balance)
        {
            
            ApplicationUser user = context.applicationUsers.Single(x => x.Id == UserId);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                user.Balance = user.Balance + balance;
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
                return Ok();
            }

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool RemoveBalance(decimal balance)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser user = context.applicationUsers.Single(x => x.Id == UserId);
            if (user == null)
            {
                return false;
            }
            else
            {
                if (user.Balance > balance)
                {
                    user.Balance = user.Balance - balance;
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }

            }
        }

    }
}
