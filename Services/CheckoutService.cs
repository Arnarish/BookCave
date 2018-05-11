using BookCave.Data.EntityModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;
using System.Collections.Generic;
using System.Web;

namespace BookCave.Services
{
    public class CheckoutService
    {
        OrderRepo _orderRepo = new OrderRepo();
        CheckoutRepo _checkRepo = new CheckoutRepo();
        BookRepo _bookRepo = new BookRepo();
        public void Add(Order order)
        {
            AddOrderDetails(order);
            _checkRepo.Add(order);
        }
        public bool ValidUserOrder(int Id, string UserName)
        {
            return _checkRepo.ValidUserOrder(Id, UserName);            
        }

        public List<UserOrderViewModel> GetOrderByUserName(string UserName)
        {
            var userOrders = _checkRepo.GetOrdersByUserName(UserName);
            
            foreach(var order in userOrders)
            {
                order.OrderDetails = _orderRepo.getOrderDetails(order.OrderId);
                foreach(var book in order.OrderDetails)
                    {
                        book.Books = _bookRepo.GetOrderDetailsBooks(book.BookId);
                    }
            }
            return userOrders;
        }
        public Order AddOrderDetails(Order order)
        {
           order.OrderDetails = _orderRepo.getOrderDetails(order.OrderId);
           foreach(var book in order.OrderDetails)
           {
               book.Books = _bookRepo.GetOrderDetailsBooks(book.BookId);
           }
           return order;
        }
    }
}