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
            var cartItem = _storeDb.Carts.SingleOrDefault(
                                        c => c.CartId == ShoppingCartId
                                        && c.BookId == book.BookId);
            if(cartItem == null)
            {
                //no items in cart
                cartItem = new Cart 
                {
                    CartId = ShoppingCartId,
                    BookId = book.BookId,
                    Count = 1,
                    DateCreated = DateTime.Now,
                };
                _storeDb.Carts.Add(cartItem);
            }
            else
            {
                //if the book exists in the cart, increase the quantity
                cartItem.Count++;
            }            
            _storeDb.SaveChanges();
        }

        public int RemoveFromCart(int id, string ShoppingCartId)
        {
            //find the correct item from the database
            var cartItem = _storeDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            int itemCount = 0;
            //decrement the item there's more than 1, else remove it.
            if(cartItem != null)
            {
                if(cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _storeDb.Carts.Remove(cartItem);
                }
            }       
            _storeDb.SaveChanges();

            return itemCount;
        }
        public int RemoveAllFromCart(int id, string ShoppingCartId)
        {
            var CartItem = _storeDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            //Ef has a remove function to do our work for us
            _storeDb.Carts.Remove(CartItem);
            //save the now removed item
            _storeDb.SaveChanges();
            //return 0, as every instance of the item has been removed
            return (int)0;
        }            
        public void EmptyCart(string ShoppingCartId)
        {
            //completely empties the cart, then saves the changes to the database
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
            // migrates a guest cart to a signed in or freshly registered user, after login
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
            //manage and store orderdetails related to the order
            foreach(var item in cartItems)
            {
                var orderDetails = new OrderDetails
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    BookQuantity = item.Count,
                    UnitPrice = Math.Round(item.Book.Price * (1-((double)item.Book.Discount / 100)), 2, MidpointRounding.AwayFromZero)
                };
                //sum up the total price of the order
                orderTotal += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.Count, 2, MidpointRounding.AwayFromZero);
                _storeDb.OrderDetails.Add(orderDetails);
            }   
            //set order total to ordertotal Count
            if(orderTotal < 50)
            {
                // add $5 to the order to acCount for shipping cost
                order.Total = Math.Round((orderTotal + (double)5), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                order.Total = orderTotal;   
            }
            //save the order
            _storeDb.Orders.Update(order);
            _storeDb.SaveChanges();
            //empty the cart
            EmptyCart(ShoppingCartId);
            //return the order id as a confirmation number
            return order.OrderId;
        }
        public List<Cart> GetCartItems(string ShoppingCartId)
        {
            //gets a list of cart items
            var item = _storeDb.Carts.Where(
                        cart => cart.CartId == ShoppingCartId)
                        .ToList();
            foreach ( var c in item ) {
                //assigns the books via eager loading
                var book = _storeDb.Books.Where( b => b.BookId == c.BookId ).Single();
                c.Book = book;
            }
            return item;
        }
        public int GetCount(string ShoppingCartId)
        {
            int? count = (from cartItems in _storeDb.Carts
                        where cartItems.CartId == ShoppingCartId
                        select (int?)cartItems.Count).Sum();
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
                    total += Math.Round((item.Book.Price * (1-((double)item.Book.Discount / 100))) * item.Count, 2);
                }
                else
                {
                    total += item.Book.Price * item.Count;
                }
            }
            //return 0 if null
            return total ?? 0;
        }
        public List<OrderDetails> getOrderDetails(int id)
        {
            //returns the orderdetails for a given orderId
            return _storeDb.OrderDetails.Where(od => od.OrderId == id).ToList();
        }
    }
}