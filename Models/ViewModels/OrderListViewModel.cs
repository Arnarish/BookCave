using System;
using System.Collections.Generic;
using BookCave.Data.EntityModels;

namespace BookCave.Models.ViewModels
{
    public class OrderListViewModel{
        public int Id { get; set; }
        public List<Book> Books {get; set;}
        public DateTime Date { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public double TotalPrice{ get; set; }
    }
}