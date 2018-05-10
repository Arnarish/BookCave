using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookCave.Data.EntityModels;

namespace BookCave.Models.InputModels
{
    public class ReviewInputModel
    {
        public string Comment { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public double BookAverageRating { get; set; }
        public int AmountOfRatings { get; set; }
    }
}