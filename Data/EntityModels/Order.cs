using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using BookCave.Models.InputModels;

namespace BookCave.Data.EntityModels
{

   public partial class Order
    {
        public int OrderId { get; set; }
        public string Username   { get; set; }
        public string FullName  { get; set; }
        public string Address    { get; set; }
        public string City       { get; set; }
        public string PostalCode { get; set; }
        public string Country    { get; set; }
        public string Email      { get; set; }
        public double Total     { get; set; }
        public System.DateTime OrderDate      { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
    }
}