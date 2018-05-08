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
        public IActionResult Index()
        {
            var randomizedBooks = _bookService.GetThreeBooksByRandom();
                                    
            return View(randomizedBooks);
        }
        [HttpGet]
        public IActionResult SearchResults(string searchString, string genre)
        {
            var bookSearch = _bookService.SearchResults(searchString, genre);

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
