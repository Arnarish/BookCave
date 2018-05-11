using BookCave.Data;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookCave.Repositories
{
    public class OrderRepo
    {
        private Datacontext _storeDb = new Datacontext();
        public void AddToCart(Book book, string ShoppingCartId)
        {   
            var CartItem = _storeDb.Carts.SingleOrDefault(
                                        c => c.CartId == ShoppingCartId
                                        && c.BookId == book.BookId);
            if(CartItem == null)
            {
                //no items in cart
                CartItem = new Cart 
                {
                    CartId = ShoppingCartId,
                    BookId = book.BookId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                };
                _storeDb.Carts.Add(CartItem);
            }
            else
            {
                //if the book exists in the cart, increase the quantity
                CartItem.Count++;
            }            
            _storeDb.SaveChanges();
        }

        public int DecBook(Cart CartItem)
        {
            int Count = 0;
            var incItem = _storeDb.Carts.SingleOrDefault(
                                                 i => i.CartId == CartItem.CartId
                                                && i.BookId == CartItem.BookId);
            if(CartItem.Count > 1)
            {
                incItem.Count--;
                Count = incItem.Count;
            }
            else
            {
                _storeDb.Carts.Remove(CartItem);
            }

            _storeDb.SaveChanges();

            return Count;
        }

        public int RemoveFromCart(int id, string ShoppingCartId)
        {
            var CartItem = _storeDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            int ItemCount = 0;
            
            if(CartItem != null)
            {
                if(CartItem.Count > 1)
                {
                    CartItem.Count--;
                    ItemCount = CartItem.Count;
                }
                else
                {
                    _storeDb.Carts.Remove(CartItem);
                }
            }       
            _storeDb.SaveChanges();

            return ItemCount;
        }
        public int RemoveAllFromCart(int id, string ShoppingCartId)
        {
            var CartItem = _storeDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            _storeDb.Carts.Remove(CartItem);
            _storeDb.SaveChanges();

            return (int)0;
        }

            
        public void EmptyCart(string ShoppingCartId)
        {
            var cartItems = _storeDb.Carts.Where(
                    cart => cart.CartId == ShoppingCartId);
            foreach(var item in cartItems)
            {
                _storeDb.Carts.Remove(item);
            }
            _storeDb.SaveChanges();
        }
        public void MigrateCart(string UserName, string ShoppingCartId)
        {
            var shoppingCart = _storeDb.Carts.Where(
                    c => c.CartId == ShoppingCartId);
            foreach(Cart item in shoppingCart)
            {
                item.CartId = UserName;
            }
            _storeDb.SaveChanges();
        }
        public int CreateOrder(Order order, string ShoppingCartId)
        {
            double orderTotal = 0;

            var cartItems = GetCartItems(ShoppingCartId);

            foreach(var item in cartItems)
            {
                var orderDetails = new OrderDetails
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    BookQuantity = item.Count,
                    UnitPrice = Math.Round(item.Book.Price * (1-((double)item.Book.Discount / 100)), 2, MidpointRounding.AwayFromZero)
                };
                
                orderTotal += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.Count, 2, MidpointRounding.AwayFromZero);
                _storeDb.OrderDetails.Add(orderDetails);
            }
            //set order total to ordertotal Count
            if(orderTotal < 50)
            {
                // add $5 to the order to acCount for shipping cost
                order.Total = Math.Round((orderTotal + (double)5), 2 ,MidpointRounding.AwayFromZero);
            }
            else
            {
                order.Total = orderTotal;   
            }
            //save the order
            _storeDb.SaveChanges();
            //empty the cart
            EmptyCart(ShoppingCartId);
            //return the order id as a confirmation number
            return order.OrderId;
        }
        public List<Cart> GetCartItems(string ShoppingCartId)
        {
            var item = _storeDb.Carts.Where(
                        cart => cart.CartId == ShoppingCartId)
                        .ToList();
            foreach ( var c in item ) {
                var book = _storeDb.Books.Where( b => b.BookId == c.BookId ).Single();
                c.Book = book;
            }
            return item;
        }
        public int GetCount(string ShoppingCartId)
        {
            int? Count = (from cartItems in _storeDb.Carts
                        where cartItems.CartId == ShoppingCartId
                        select (int?)cartItems.Count).Sum();
            //return 0 if all entries are null
            return Count ?? 0;
        }
        public double GetTotal(string ShoppingCartId)
        {
            var stuff = GetCartItems(ShoppingCartId);
            double? total = 0;
            foreach (var item in stuff)
            {
                if(item.Book.OnSale)
                {
                    total += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.Count, 2);
                }
                else
                {
                    total += item.Book.Price * item.Count;
                }
            }
            return total ?? 0;
        }
        public List<OrderDetails> getOrderDetails(int id)
        {
            return _storeDb.OrderDetails.Where(od => od.OrderId == id).ToList();
        }
    }
}