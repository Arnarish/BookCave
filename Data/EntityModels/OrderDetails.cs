using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BookCave.Data.EntityModels;

namespace BookCave.Data.EntityModels
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int BookQuantity { get; set; }
        public double UnitPrice { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Books { get; set; }
    }
}