using BookCave.Data;
using BookCave.Data.EntityModels;
using System.Linq;
using System.Web;

namespace BookCave.Repositories
{
    public class CheckoutRepo
    {
        private Datacontext _DB = new Datacontext();

        public void Add (Order order)
        {
            _DB.Orders.Add(order);
            _DB.SaveChanges();
        }
        public bool ValidUserOrder(int Id, string UserName)
        {
            bool IsValid = _DB.Orders.Any(
                        o => o.OrderId == Id
                        && o.Username == UserName);
            return IsValid;
        }
    }
}