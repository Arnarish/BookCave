using System;
using System.Collections.Generic;
using BookCave.Data.EntityModels;

namespace BookCave.Models.ViewModels
{
    public class OrderListViewModel{
        public int OrderId { get; set; }
        public List<Book> Books { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}