using System.ComponentModel;
using System.Web;
namespace BookCave.Data.EntityModels
{

    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
        public int FavoriteBookById { get; set; }
    }
}