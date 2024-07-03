using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Ecommerce_web_API.Models
{
    public class ApplicationDbContext:IdentityDbContext

    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
       
        public DbSet<ShoppingCart> ShoppingCart { get; set;}
        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderItems>().HasKey(oi => new { oi.OrderId, oi.ProductId });
            modelBuilder.Entity<ShoppingCartItems>().HasKey(sci => new { sci.ShoppingCartId, sci.ProductId });

        }
    }
    
    


}
