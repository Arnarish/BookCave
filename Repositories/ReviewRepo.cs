using System.Collections.Generic;
using System.Linq;
using BookCave.Data;
using BookCave.Data.EntityModels;
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
        public List<ReviewListViewModel> GetAllReviewsByBookID(int? id)
        {
            var bookreview = (from a in _db.Reviews
                                where id == a.BookId
                                select new ReviewListViewModel
                                {
                                    ReviewId = a.ReviewId,
                                    Comment = a.Comment,
                                    Rating = a.Rating,
                                    UserId = a.UserId,
                                    BookId = a.BookId,
                                }).ToList();

            return bookreview;
        }
        public void AddReview(Review review)
        {
            _db.Add(review);
            _db.SaveChanges();
        }
    }
}