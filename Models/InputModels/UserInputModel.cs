namespace BookCave.Models.InputModels
{
    public class UserInputModel
    {
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public int Postal { get; set; }
        public int FavoriteBookById { get; set; }
    }
}