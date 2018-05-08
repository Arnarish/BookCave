using System.Collections.Generic;
using BookCave.Data.EntityModels;

namespace BookCave.Models.InputModels
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
        public double UnitPrice { get; set; }
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }
    }
}