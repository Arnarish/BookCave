using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookCave.Data.EntityModels;

namespace BookCave.Models.ViewModels
{
    public class UserOrderViewModel
    {
        [Key]
         public int OrderId { get; set; }
        public string Username   { get; set; }
        public string FullName  { get; set; }
        public string Address    { get; set; }
        public string City       { get; set; }
        public string PostalCode { get; set; }
        public string Country    { get; set; }
        public string Email      { get; set; }
        public double Total     { get; set; }
        public DateTime OrderDate      { get; set; }
        [ForeignKey("OrderId")]
        public List<OrderDetails> OrderDetails { get; set; }
    }
}