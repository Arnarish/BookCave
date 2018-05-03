using System.ComponentModel;
using System.Web;
namespace BookCave.Data.EntityModels
{

    public class Review{
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}