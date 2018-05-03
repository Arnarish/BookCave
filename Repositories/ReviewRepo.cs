using System.Collections.Generic;
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
            var reviews = (from a in _db.reviews
                            join b in _db.books on a.BookId equals b.BookId
                            join u in _db.users on a.UserId equals base.UserId
                            select new ReviewListViewModel
                            {
                                ReviewID = a.ReviewID,
                                Comment = a.Comment,
                                Rating = a.Rating,
                                
                            });
                return reviews;
        }*/
    }
}