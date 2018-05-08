using System.Collections.Generic;
using BookCave.Models.InputModels;

namespace BookCave.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public double CartTotal { get; set; }
    }
}