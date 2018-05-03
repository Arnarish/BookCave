using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
namespace BookCave.Data.EntityModels
{

    public class Order{
        public int Id { get; set; }
        public List<Book> Books {get; set;}
        public DateTime Date { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public double TotalPrice{ get; set; }
    }
}