using System.Collections.Generic;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services
{
    public class ReviewService
    {
        private ReviewRepo _ReviewRepo;

        public ReviewService()
        {
            _ReviewRepo = new ReviewRepo();
        }
        public List<ReviewListViewModel> GetAllReviewsByBookID(int? id)
        {
            var book = _ReviewRepo.GetAllReviewsByBookID(id);

            return book;
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
            _ReviewRepo.AddReview(review);
        }
    }
}