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
                    new Book 
                        { Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams", ReleaseYear = 1979, Genre = "SciFi", ISBN = "330258648", Price = 17.99, Stock = 10, TopSeller = false, OnSale = false, Discount = 0, Image = "https://images.penguinrandomhouse.com/cover/9781400052929"},
                    new Book 
                        { Title = "IT", Author = "Stephen King", ReleaseYear = 1986, Genre = "Horror", ISBN = "670813028", Price = 23.99, Stock = 15, TopSeller = false, OnSale = false, Discount = 0, Image = "http://cdn.collider.com/wp-content/uploads/2017/09/it-book-cover.jpg"}

                };

                db.AddRange(InitialBooks);
                db.SaveChanges();
            }
        }
    }    
}
