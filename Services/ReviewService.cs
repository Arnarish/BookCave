using System.Collections.Generic;
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
    }
}