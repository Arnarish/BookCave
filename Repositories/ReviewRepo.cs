using System;
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
            foreach (var r in bookreview)
            {
                var user = _db.Users.Where( u => u.UserId == r.UserId).Single();
                r.User = user;
            }

            return bookreview;
        }
        public void AddReview(Review review, double average, int amount)
        {
            _db.Reviews.Add(review);
            var book = (from b in _db.Books
                        where b.BookId == review.BookId
                        select b).SingleOrDefault();
            book.ReviewScore = Math.Round((review.Rating + average * amount) / (amount + 1), 2);
            _db.SaveChanges();
        }
        public List<ReviewListViewModel> GetAllReviewsByUserID(int UserId)
        {
            var userReviews = (from a in _db.Reviews
                                where UserId == a.UserId
                                select new ReviewListViewModel
                                {
                                    ReviewId = a.ReviewId,
                                    Comment = a.Comment,
                                    Rating = a.Rating,
                                    UserId = a.UserId,
                                    BookId = a.BookId,
                                }).ToList();
            foreach (var r in userReviews)
            {
                var book = _db.Books.Where( u => u.BookId == r.BookId).Single();
                r.Book = book;
            }
            return userReviews;
        }
    }
}