using System.Collections.Generic;
using System.Linq;
using BookCave.Data;
using BookCave.Models.ViewModels;

namespace BookCave.Repositories
{
    public class ReviewRepo
    {
        private Datacontext _db;

        public ReviewRepo()
        {
            _db = new Datacontext();
        }
        
        public BookAndReviewListViewModel GetBookWithAllReviews(int id)
        {
            var bookreview = (from m in _db.Books
                                where id == m.BookId
                                select new BookAndReviewListViewModel{
                                BookId = m.BookId,
                                Title = m.Title,
                                Author = m.Author,
                                ReleaseYear = m.ReleaseYear,
                                Genre = m.Genre,
                                ISBN = m.ISBN,
                                Price = m.Price,
                                Stock = m.Stock,
                                TopSeller = m.TopSeller,
                                OnSale = m.OnSale,
                                Discount = m.Discount,
                                Image = m.Image,
                                Reviews = (from a in _db.Reviews
                                            where m.BookId == a.ReviewId
                                            select new ReviewListViewModel
                                            {
                                                ReviewId = a.ReviewId,
                                                Comment = a.Comment,
                                                Rating = a.Rating,
                                                UserId = a.UserId,
                                                BookId = a.BookId,
                                            }).ToList()
                                }).SingleOrDefault();
            return bookreview;
        }
    }
}