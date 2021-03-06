using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using BookCave.Models.ViewModels;
using BookCave.Models.InputModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookCave.Controllers
{
    public class BookController : Controller    
    {
        private BookService _bookService;
        private ReviewService _reviewService;
        private UserService _userService;
        
        public BookController()
        {
            _bookService = new BookService();
            _reviewService = new ReviewService();
            _userService = new UserService();
        }
        public IActionResult Index()
        {
            var books = _bookService.GetAllBooks();
            
            return View(books);
        }
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return View("Index");
            }
            var book = _bookService.GetBookById(id);
            var reviews = _reviewService.GetAllReviewsByBookID(id);
            reviews.Reverse();
            if(book == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var bookAndReviews = new BookAndReviewListViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ReleaseYear = book.ReleaseYear,
                Genre = book.Genre,
                ISBN = book.ISBN,
                Price = book.Price,
                Stock = book.Stock,
                ReviewScore = book.ReviewScore,
                TopSeller = book.TopSeller,
                OnSale = book.OnSale,
                Discount = book.Discount,
                Image = book.Image,
                Reviews = reviews
            };
            if(bookAndReviews == null)
            {
                return View("Index");
            }
            return View(bookAndReviews);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ReviewInputModel model)
        {
            //we use the details httppost to add a review to the database
            if(model.Comment == null)
            {
                model.Comment = "[User left no review]";
            }
            if(!ModelState.IsValid)
            {
                return View("/Book/Details/" + model.BookId);
            }
            
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var user = _userService.GetUserViewModelByString(claim);
            model.UserId = user.UserId;
            _reviewService.AddReview(model);
            return RedirectToAction("Details", model.BookId);
        }
        public IActionResult AuthorDetails(int? id)
        {
            var books = _bookService.GetBooksByAuthor(id);
            return View(books);    
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(BookInputModel model)
        {
            //This adds another book to the database
            if(!ModelState.IsValid)
            {
                return View();
            }
            _bookService.AddBook(model);
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(int? id)
        {   
            var book = _bookService.GetBookViewModelById(id);
            return View(book);
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Edit(BookInputModel model)
        {
            //This updates the Book based on the changes made
            if(!ModelState.IsValid)
            {
                return View();
            }
            model.Price = Convert.ToDouble(model.Price);
            var book = _bookService.GetBookById(model.BookId);
            _bookService.UpdateBook(book, model);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            //Get the Book then remove it
            var book = _bookService.GetBookById(id);
            _bookService.RemoveBook(book);
            return RedirectToAction("Index", "Home");
        }
    }
}
