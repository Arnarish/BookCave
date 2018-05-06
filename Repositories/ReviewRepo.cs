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

        /*public List<ReviewListViewModel> GetAllReviews()
        {
            var reviews = (from a in _db.Reviews
                            join b in _db.Books on a.BookId equals b.BookId
                            join u in _db.Users on a.UserId equals u.UserId
                            select new ReviewListViewModel
                            {
                                ReviewId = a.ReviewId,
                                Comment = a.Comment,
                                Rating = a.Rating,
                                UserId = u.UserId,
                                BookId = b.BookId                                
                            }).ToList();
                return reviews;
        }*/
    }
}