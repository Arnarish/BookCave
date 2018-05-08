using System.Collections.Generic;

namespace BookCave.Models.ViewModels
{

    public class BookAndReviewListViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public bool TopSeller { get; set; }
        public bool OnSale { get; set; }
        public int Discount { get; set; }
        public string Image { get; set; }
        public List<ReviewListViewModel> Reviews { get; set;}

    }
}