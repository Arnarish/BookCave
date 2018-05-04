using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookCave.Data;
using BookCave.Data.EntityModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookCave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            SeedData();
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void SeedData()
        {
            var db = new Datacontext();
            if(!db.Books.Any())
            {
                var InitialBooks = new List<Book>()
                {
                    //10 default books, Sindri
                    new Book 
                        { Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams", ReleaseYear = 1979, Genre = "SciFi", ISBN = "330258648", Price = 17.99, Stock = 10, TopSeller = false, OnSale = false, Discount = 0, Image = "https://images.penguinrandomhouse.com/cover/9781400052929"},
                    new Book 
                        { Title = "It", Author = "Stephen King", ReleaseYear = 1986, Genre = "Horror", ISBN = "670813028", Price = 23.99, Stock = 15, TopSeller = false, OnSale = false, Discount = 0, Image = "http://cdn.collider.com/wp-content/uploads/2017/09/it-book-cover.jpg"},
                    new Book 
                        { Title = "The Dark Knight Returns", Author = "Frank Miller", ReleaseYear = 1986, Genre = "Comics", ISBN = "1563893428", Price = 20.99, Stock = 25, TopSeller = false, OnSale = false, Discount = 0, Image = "https://upload.wikimedia.org/wikipedia/en/7/77/Dark_knight_returns.jpg"},
                    new Book 
                        { Title = "The Shining", Author = "Stephen King", ReleaseYear = 1977, Genre = "Horror", ISBN = "9780385121675", Price = 10.99, Stock = 20, TopSeller = false, OnSale = false, Discount = 0, Image = "https://upload.wikimedia.org/wikipedia/en/4/4c/Shiningnovel.jpg"},
                    new Book 
                        { Title = "Stars in My Pocket Like Grains of Sand", Author = "Samuel R. Delany", ReleaseYear = 1984, Genre = "SciFi", ISBN = "9780553050530", Price = 27.99, Stock = 10, TopSeller = false, OnSale = false, Discount = 0, Image = "https://upload.wikimedia.org/wikipedia/en/4/4d/Stars_in_my_pocket_like_grains_of_sand.jpg"},
                    new Book 
                        { Title = "The Grapes of Wrath", Author = "John Steinbeck", ReleaseYear = 1939, Genre = "Drama", ISBN = "9780143039433", Price = 11.49, Stock = 7, TopSeller = false, OnSale = false, Discount = 0, Image = "https://upload.wikimedia.org/wikipedia/en/1/1f/JohnSteinbeck_TheGrapesOfWrath.jpg"},
                    new Book 
                        { Title = "How to Compile C Code", Author = "Bjorgvin Karason", ReleaseYear = 2013, Genre = "Educational", ISBN = "9780446310001", Price = 44.29, Stock = 13, TopSeller = false, OnSale = false, Discount = 0, Image = "https://images-na.ssl-images-amazon.com/images/I/41gpxHEh2rL._SX403_BO1,204,203,200_.jpg"},
                    new Book 
                        { Title = "The Hobbit, or There and Back Again", Author = "J. R. R. Tolkien", ReleaseYear = 1937, Genre = "Fantasy", ISBN = "9780547928227", Price = 27.09, Stock = 50, TopSeller = false, OnSale = false, Discount = 0, Image = "http://media.bookbub.com/wp-content/uploads/2014/12/1937.jpg"},
                    new Book 
                        { Title = "To Kill a Mockingbird", Author = "Harper Lee", ReleaseYear = 1960, Genre = "Drama", ISBN = "9780446310789", Price = 23.99, Stock = 15, TopSeller = false, OnSale = false, Discount = 0, Image = "https://upload.wikimedia.org/wikipedia/en/7/79/To_Kill_a_Mockingbird.JPG"}
                     };
                db.AddRange(InitialBooks);
                db.SaveChanges();
            }
        }
    }    
}
