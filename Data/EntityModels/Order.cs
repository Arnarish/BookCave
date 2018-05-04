using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
namespace BookCave.Data.EntityModels
{

    public class Order{
        public int OrderId { get; set; }
        public List<Book> Books { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
    }
}