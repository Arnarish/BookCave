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
using BookCave.Data.EntityModels;

namespace BookCave.Controllers
{
    public class BookController : Controller    
    {
        private BookService _bookService;
        private ReviewService _reviewService;

        
        public BookController()
        {
            _bookService = new BookService();
            _reviewService = new ReviewService();
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
        
        public IActionResult AuthorDetails(int? id)
        {

                var books = _bookService.GetBooksByAuthor(id);

                return View(books);    
        }

        public IActionResult AddBookToWaitingList(int? id)
        {

            var book = _bookService.GetBookById(id);
            
            return View();
        }
        
        /*[HttpPost]
        public IActionResult CreateNewReview(ReviewInputModel review)
        {
                
                if(ModelState.IsValid)
                {
                    var newReview = new Review(){
                        Comment = review.Comment,
                        Rating = review.Rating,
                        UserId = review.UserId,
                        BookId = review.BookId
                };
                //_db.ReviewRepo.Add(newReview);
                return RedirectToAction("Details");
                }
            return View();
        } todo*/
        
    }
}
