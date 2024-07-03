using Ecommerce_web_API.Models;
using Ecommerce_web_API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ecommerce_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/<ProductController>
        [HttpGet]

        public IActionResult Get()
        {
            return Ok(context.Products.ToList());
        }

        [HttpGet]
        [Route("api/products/GetSellerProducts")]
        
        public IActionResult GetSellerProducts(string Seller)
        {

            return Ok(context.Products.Where(x => x.SellerId == Seller));
        }

        // GET api/<ProductController>/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok(context.Products.FirstOrDefault(x => x.Id == id));
        }




        // POST api/<ProductController>
        [HttpPost]
        [Authorize(Roles = "Seller")]

        public IActionResult AddProduct(AddProductViewModel newProduct)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Name = newProduct.Name;
                product.Description = newProduct.Description;
                product.Price = newProduct.Price;
                product.Cost = newProduct.Cost;
                product.Amount = newProduct.Amount;
                product.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.UserName = User.Identity.Name;

                context.Products.Add(product);


                return Ok(context.SaveChanges());
            }
            else
            {
                return BadRequest();
            }


        }

       // PUT api/<ProductController>/5
        [HttpPut]
        public IActionResult Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            context.Entry(product).State = EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    throw;
                
            }

            return NoContent();
        }

       
    
        
   
        // DELETE api/<ProductController>/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (context.Products == null)
            {
                return NotFound();
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Products.Remove(product);
            context.SaveChanges();

            return NoContent();
        }
        
    }
}
