using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models;
using WebApplicationTest.Models.Identity;

namespace WebApplicationTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<DeliveryOption> DeliveryOptions { get; set; }

    }
}
