using System;
using System.Collections.Generic;
using BookCave.Models.InputModels;
using BookCave.Data.EntityModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services
{
    public class ReviewService
    {
        private ReviewRepo _reviewRepo;

        public ReviewService()
        {
            _reviewRepo = new ReviewRepo();
        }
        public List<ReviewListViewModel> GetAllReviewsByBookID(int? id)
        {
            var reviews = _reviewRepo.GetAllReviewsByBookID(id);

            return reviews;
        }
        public void AddReview(ReviewInputModel model)
        {
            var review = new Review
            {
                Comment = model.Comment,
                Rating = model.Rating,
                UserId = model.UserId,
                BookId = model.BookId
            };
            _reviewRepo.AddReview(review, model.BookAverageRating, model.AmountOfRatings);
        }
        public UserViewModel AddReviewsToViewModel(UserViewModel model)
        {
            model.Reviews = _reviewRepo.GetAllReviewsByUserID(model.UserId);
            model.Reviews.Reverse();
            return model;
        }
    }
}