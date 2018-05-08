using System;
using System.ComponentModel.DataAnnotations;
using BookCave.Data.EntityModels;

namespace BookCave.Models.InputModels
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string CartId { get; set; }
        public int BookId { get; set; }
        public int count { get; set; }
        public DateTime DateCreated  { get; set; }
        public virtual Book Book { get; set; }
    }
}