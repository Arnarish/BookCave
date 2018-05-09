using System.Collections.Generic;

namespace BookCave.Models.ViewModels
{

    public class UserAndReviewListViewModel
    {
    
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public int Postal { get; set; }
        public int FavoriteBookById { get; set; }

    }
}