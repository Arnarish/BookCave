using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using BookCave.Models.ViewModels;

namespace BookCave.Controllers
{
    public class BookController : Controller    
    {
        private BookService _bookService;

        public BookController()
        {
            _bookService = new BookService();
        }
        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            
            return View(books);
        }
        public IActionResult Details(int? id)
        {   
            var filteredId = _bookService.GetAllBooks().FirstOrDefault(b => b.BookId == id);
                return View(filteredId);
        }

        [HttpGet]
        public IActionResult AuthorDetails(string name)
        {
            var books = _bookService.GetAllBooks();

            var filteredAuthor = (from b in books
                            where b.Author == name
                            orderby b.ReleaseYear
                            select new BookListViewModel
                            {
                                BookId = b.BookId,
                                Author = b.Author,
                                Title = b.Title,
                                ReleaseYear = b.ReleaseYear,
                                Genre = b.Genre,
                                Price = b.Price,
                                Image = b.Image
                            }).ToList();

             if(name == null)
            {
                return View();    
            }
            return View(filteredAuthor);
        }

        
    }
}
