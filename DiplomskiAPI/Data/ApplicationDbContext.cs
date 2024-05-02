using DiplomskiAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiplomskiAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> Korisnici { get; set; }

        public DbSet<Lek> Lekovi { get; set; }

        public DbSet<CartItem> CartItems { get; set; }


        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Lek>().HasData(
                new Lek
                {
                    LekID = 1,
                    NazivLeka = "Arginine",
                    Description = "60 Veggie Capsules",
                    Price = 9.80,
                    ISBN = "11111",
                    MainCategory = "Supplements",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/arginine.png"
                }, new Lek
                {
                    LekID = 2,
                    NazivLeka = "5-HTP Melatonin",
                    Description = "30 Veggies Capsules",
                    Price = 28.42,
                    ISBN = "11112",
                    MainCategory = "5-HTP",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/5HTPMelatonin.png"
                }, new Lek
                {
                    LekID = 3,
                    NazivLeka = "5-HTP",
                    Description = "100mg 120 Veggie Capsules",
                    Price = 19.97,
                    ISBN = "11113",
                    MainCategory = "5-HTP",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/5-htp-.png"
                }, new Lek
                {
                    LekID = 4,
                    NazivLeka = "Calcium, Mg, Zinc + D3",
                    Description = "90 Tablets",
                    Price = 4.16,
                    ISBN = "11114",
                    MainCategory = "Minerals",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/Calcium, Mg, Zinc, D3.png"
                }, new Lek
                {
                    LekID = 5,
                    NazivLeka = "Omega-3",
                    Description = "120 DHA per Softgel, 100 SoftGels",
                    Price = 6.72,
                    ISBN = "11115",
                    MainCategory = "Omega-3",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/omega-3.png"
                },
                new Lek
                {
                    LekID = 6,
                    NazivLeka = "Vitamin C",
                    Description = "120 DHA per Softgel, 100 SoftGels",
                    Price = 6.72,
                    ISBN = "11118",
                    MainCategory = "Omega-3",
                    BestSeller = true,
                    TimesBought = 0,
                    Image = "https://diplomskislike.blob.core.windows.net/diplomski/cvitamin.png"
                });   
        }
    }
}
