using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using Microsoft.EntityFrameworkCore;

namespace BookCave.Data
{
    public class Datacontext : DbContext //laga í camel casing?
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseSqlServer(
                        "Server=tcp:verklegt2.database.windows.net,1433;Initial Catalog=VLN2_2018_H27;Persist Security Info=False;User ID=VLN2_2018_H27_usr;Password=keenFi$h46;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
            }
    }
}