namespace BookCave.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public int Postal { get; set; }
        public string FavoriteBook { get; set; }
        public int FavoriteBookId { get; set; }
    }
}