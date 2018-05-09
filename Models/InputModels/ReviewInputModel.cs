using System.Collections.Generic;
using BookCave.Data.EntityModels;

namespace BookCave.Models.InputModels
{
    public class ReviewInputModel
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}