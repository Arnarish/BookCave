namespace BookCave.Models.ViewModels
{
    public class UserListViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int FavoriteBookById { get; set; }
        public bool IsStaff { get; set; }
    }
}