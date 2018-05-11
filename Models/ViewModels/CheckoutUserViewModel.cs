using System;
using System.Collections.Generic;
using BookCave.Data.EntityModels;

namespace BookCave.Models.ViewModels
{
    public class CheckoutUserViewModel
    {
        public string FullName  { get; set; }
        public string Address    { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email      { get; set; }
    }
}