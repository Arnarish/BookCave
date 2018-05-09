using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCave.Data.EntityModels
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string CartId { get; set; }
        public int BookId { get; set; }
        public int count { get; set; }
        public DateTime DateCreated  { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}