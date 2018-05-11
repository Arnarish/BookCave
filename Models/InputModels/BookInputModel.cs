using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{

    public class BookInputModel
    {
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public bool TopSeller { get; set; }
        public bool OnSale { get; set; }
        [Required]
        [Range(0, 100)]
        public int Discount { get; set; }
        [Required]
        public string Image { get; set; }
    }
}