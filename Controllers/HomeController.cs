using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookCave.Controllers
{
<<<<<<< HEAD
    [Authorize]
=======
    //[Authorize] //bilað
>>>>>>> 897048c802210d6edc9786b9a33de517efaae5f3
    public class HomeController : Controller    
    {
        private BookService _bookService;

        public HomeController()
        {
            _bookService = new BookService();
        }
        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            
            return View(books);
        }
        public IActionResult About()
        {   
            return View();
        }
        public IActionResult TopTen()
        {
            var topTenBooks = _bookService.GetAllBooks().Take(10);

            return View(topTenBooks);

        }
    }
}
