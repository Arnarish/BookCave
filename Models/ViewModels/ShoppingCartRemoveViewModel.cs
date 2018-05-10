namespace BookCave.Models.ViewModels
{
    public class ShoppingCartRemoveViewModel
    {
        public double CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}