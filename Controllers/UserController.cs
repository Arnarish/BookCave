using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;

namespace BookCave.Controllers
{
    public class UserController : Controller    
    {
        //private UserService _userService;

        public UserController() 
        {
            //_bookService = new BookService();
        }
       /* public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            
            return View(books);
        }
        public IActionResult About()
        {   
            return View();
        }*/
    }
}
