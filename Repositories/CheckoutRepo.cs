using BookCave.Data;
using BookCave.Data.EntityModels;
using BookCave.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCave.Repositories
{
    public class CheckoutRepo
    {
        private Datacontext _db = new Datacontext();

        public void Add (Order order)
        {
            //add a order to the database.
            _db.Orders.Add(order);
            _db.SaveChanges();
        }
        public bool ValidUserOrder(int Id, string UserName)
        {
            //checks if the order is valid based on the user
            bool IsValid = _db.Orders.Any(
                        o => o.OrderId == Id
                        && o.Username == UserName);
            return IsValid;
        }
        public List<UserOrderViewModel> GetOrdersByUserName(string UserName)
        {
            //returns all orders for a given username as a viewmodel
            var UserOrders = (from u in _db.Orders
                    where u.Username == UserName
                    select new UserOrderViewModel
                    {
                        OrderId = u.OrderId,
                        Username = u.Username,
                        FullName = u.FullName,
                        Address = u.Address,
                        City = u.City,
                        PostalCode = u.PostalCode,
                        Country = u.Country,
                        Email = u.Email,
                        Total = u.Total,
                        OrderDate = u.OrderDate
                    }).ToList();
                    
                    return UserOrders;
        }
    }
}