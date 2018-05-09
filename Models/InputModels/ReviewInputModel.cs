using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookCave.Data.EntityModels;

namespace BookCave.Models.InputModels
{
    public class ReviewInputModel
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}