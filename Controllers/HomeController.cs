using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;
using BookCave.Models.ViewModels;

namespace BookCave.Controllers
{
    public class HomeController : Controller    
    {
        private BookService _bookService;

        public HomeController()
        {
            _bookService = new BookService();
        }
        [HttpGet]
        public IActionResult Index(string searchString, string genre)
        {
            var books = _bookService.GetAllBooks();
            if(searchString == null)
            {
                searchString = "";
            }
            if(genre == null)
            {
                genre = "";
            }
            var bookSearch = (from b in books
                            where (b.ISBN.Contains(searchString) || 
                            b.Title.ToLower().Contains(searchString.ToLower()) || 
                            b.Author.ToLower().Contains(searchString.ToLower())) &&
                            b.Genre.ToLower().Contains(genre.ToLower())
                            orderby b.Title
                            select new BookListViewModel
                            {
                                BookId = b.BookId,
                                Author = b.Author,
                                Title = b.Title,
                                Genre = b.Genre,
                                Price = b.Price,
                                Image = b.Image
                            }).ToList();
            if(bookSearch == null)
            {
                return View();    
            }
            return View(bookSearch);
        }
        public IActionResult About()
        {   
            return View();
        }
        public IActionResult TopTen()
        {
            var topTenBooks = _bookService.GetAllBooks().Take(10).ToList();

            return View(topTenBooks);

        }
    }
}
