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
        private Datacontext _StoreDb = new Datacontext();
        public void AddToCart(Book book, string ShoppingCartId)
        {   
            var CartItem = _StoreDb.Carts.SingleOrDefault(
                                        c => c.CartId == ShoppingCartId
                                        && c.BookId == book.BookId);
            if(CartItem == null)
            {
                //no items in cart
                CartItem = new Cart 
                {
                    CartId = ShoppingCartId,
                    BookId = book.BookId,
                    count = 1,
                    DateCreated = DateTime.Now,
                };
                _StoreDb.Carts.Add(CartItem);
            }
            else
            {
                //if the book exists in the cart, increase the quantity
                CartItem.count++;
            }            
            _StoreDb.SaveChanges();
        }

        public int DecBook(Cart CartItem)
        {
            int count = 0;
            var incItem = _StoreDb.Carts.SingleOrDefault(
                                                 i => i.CartId == CartItem.CartId
                                                && i.BookId == CartItem.BookId);
            if(CartItem.count > 1)
            {
                incItem.count--;
                count = incItem.count;
            }
            else
            {
                _StoreDb.Carts.Remove(CartItem);
            }

            _StoreDb.SaveChanges();

            return count;
        }

        public int RemoveFromCart(int id, string ShoppingCartId)
        {
            var CartItem = _StoreDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            int ItemCount = 0;
            
            if(CartItem != null)
            {
                if(CartItem.count > 1)
                {
                    CartItem.count--;
                    ItemCount = CartItem.count;
                }
                else
                {
                    _StoreDb.Carts.Remove(CartItem);
                }
            }       
            _StoreDb.SaveChanges();

            return ItemCount;
        }
        public int RemoveAllFromCart(int id, string ShoppingCartId)
        {
            var CartItem = _StoreDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            _StoreDb.Carts.Remove(CartItem);
            _StoreDb.SaveChanges();

            return (int)0;
        }

            
        public void EmptyCart(string ShoppingCartId)
        {
            var cartItems = _StoreDb.Carts.Where(
                    cart => cart.CartId == ShoppingCartId);
            foreach(var item in cartItems)
            {
                _StoreDb.Carts.Remove(item);
            }
            _StoreDb.SaveChanges();
        }
        public void MigrateCart(string UserName, string ShoppingCartId)
        {
            var shoppingCart = _StoreDb.Carts.Where(
                    c => c.CartId == ShoppingCartId);
            foreach(Cart item in shoppingCart)
            {
                item.CartId = UserName;
            }
            _StoreDb.SaveChanges();
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
                    BookQuantity = item.count,
                    UnitPrice = Math.Round(item.Book.Price * (1-((double)item.Book.Discount / 100)))
                };
                
                orderTotal += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.count, 2);
                _StoreDb.OrderDetails.Add(orderDetails);
            }
            //set order total to ordertotal count
            if(orderTotal < 50)
            {
                // add $5 to the order to account for shipping cost
                order.Total = (orderTotal + (double)5);
            }
            else
            {
                order.Total = orderTotal;
            }
            //save the order
            _StoreDb.SaveChanges();
            //empty the cart
            EmptyCart(ShoppingCartId);
            //return the order id as a confirmation number
            return order.OrderId;
        }
        public List<Cart> GetCartItems(string ShoppingCartId)
        {
            var item = _StoreDb.Carts.Where(
                        cart => cart.CartId == ShoppingCartId)
                        .ToList();
            foreach ( var c in item ) {
                var book = _StoreDb.Books.Where( b => b.BookId == c.BookId ).Single();
                c.Book = book;
            }
            return item;
        }
        public int GetCount(string ShoppingCartId)
        {
            int? count = (from cartItems in _StoreDb.Carts
                        where cartItems.CartId == ShoppingCartId
                        select (int?)cartItems.count).Sum();
            //return 0 if all entries are null
            return count ?? 0;
        }
        public double GetTotal(string ShoppingCartId)
        {
            var stuff = GetCartItems(ShoppingCartId);
            double? total = 0;
            foreach (var item in stuff)
            {
                if(item.Book.OnSale)
                {
                    total += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.count, 2);
                }
                else
                {
                    total += item.Book.Price * item.count;
                }
            }
            return total ?? 0;
        }
    }
}