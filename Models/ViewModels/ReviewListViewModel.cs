namespace BookCave.Models.ViewModels
{
    public class ReviewListViewModel
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
