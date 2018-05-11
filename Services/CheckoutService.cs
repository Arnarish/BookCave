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
            //adds the orderdetails to the order, then adds the order to the database
            AddOrderDetails(order);
            _checkRepo.Add(order);
        }
        public bool ValidUserOrder(int Id, string UserName)
        {
            //checks that the order is valid for the logged in user
            return _checkRepo.ValidUserOrder(Id, UserName);            
        }

        public List<UserOrderViewModel> GetOrderByUserName(string UserName)
        {
            //Retrieves all orders for a given username
            var userOrders = _checkRepo.GetOrdersByUserName(UserName);
            
            foreach(var order in userOrders)
            {
                //gets all orderdetails for the given order
                order.OrderDetails = _orderRepo.getOrderDetails(order.OrderId);
                foreach(var book in order.OrderDetails)
                    {
                        //gets all the books for each orderdetail
                        book.Books = _bookRepo.GetOrderDetailsBooks(book.BookId);
                    }
            }
            return userOrders;
        }
        public Order AddOrderDetails(Order order)
        {
            //gets orderdetails and books for the orderdetails for a given order
           order.OrderDetails = _orderRepo.getOrderDetails(order.OrderId);
           foreach(var book in order.OrderDetails)
           {
               book.Books = _bookRepo.GetOrderDetailsBooks(book.BookId);
           }
           return order;
        }
    }
}