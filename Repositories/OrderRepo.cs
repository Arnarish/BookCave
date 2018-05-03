using System.Collections.Generic;
using System.Linq;
using BookCave.Data;
using BookCave.Models.ViewModels;

namespace BookCave.Repositories
{
    public class OrderRepo
    {
        private Datacontext _db;
        public OrderRepo()
        {
            _db = new Datacontext();
        }
        public List<OrderListViewModel> GetAllOrders()
        {
            var orders = (from a in _db.Orders
                            join u in _db.Users on a.UserId equals u.UserId
                            select new OrderListViewModel
                            {
                                OrderId = a.OrderId,
                                Books = {  },
                                Date = a.Date,
                                UserId = u.UserId
                            }).ToList();
            return orders;
        }
    }
}