namespace BookCave.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int FavoriteBookById { get; set; }
    }
}