using BookCave.Data;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookCave.Services
{
    public partial class OrderService
    {
        private Datacontext _StoreDb = new Datacontext();
        private OrderRepo _OrderRep = new OrderRepo();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public static OrderService GetCart(HttpContext context)
        {
            var cart = new OrderService();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static OrderService GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Book book)
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
                _OrderRep.AddToCart(CartItem);
            }
            else
            {
                //item exists in cart, increase quantity
                _OrderRep.IncBook(CartItem);
            }
        }
        public int RemoveFromCart(int id)
        {
            //move to repo
            //get the cart
            var CartItem = _StoreDb.Carts.SingleOrDefault(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            int ItemCount = 0;
            if(CartItem != null)
            {
                if(CartItem.count > 1)
                {
                    ItemCount = CartItem.count;
                }
                else
                {
                    ItemCount = _OrderRep.RemoveFromCart(CartItem);
                }
            }
            return ItemCount;
        }
        public void EmptyCart()
        {
            //move to repo
            var cartItems = _StoreDb.Carts.Where(
                    cart => cart.CartId == ShoppingCartId);
            foreach(var cartitem in cartItems)
            {
                _StoreDb.Carts.Remove(cartitem);
            }
            _StoreDb.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            //move to repo
            var item = _StoreDb.Carts.Where(
                        cart => cart.CartId == ShoppingCartId)
                        //.Include("Books")
                        .ToList();
            foreach ( var c in item ) {
                var book = _StoreDb.Books.Where( b => b.BookId == c.BookId ).Single();
                c.Book = book;
            }
            return item;
        }
        public int GetCount()
        {
            //move to repo
            int? count = (from cartItems in _StoreDb.Carts
                        where cartItems.CartId == ShoppingCartId
                        select (int?)cartItems.count).Sum();
            //return 0 if all entries are null
            return count ?? 0;
        }
        public double GetTotal()
        {
            //move to repo
            double? total = (from cartItems in _StoreDb.Carts
                                where cartItems.CartId == ShoppingCartId
                                select (int?)cartItems.count * cartItems.Book.Price).Sum();

            return total ?? 0;
        }
        public int CreateOrder(Order order)
        {
            //move to repo
            double orderTotal = 0;

            var cartItems = GetCartItems();

            foreach(var item in cartItems)
            {
                var orderDetails = new OrderDetails
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    BookQuantity = item.count,
                    UnitPrice = item.Book.Price
                };
                orderTotal += (item.count * item.Book.Price);
                _StoreDb.OrderDetails.Add(orderDetails);
            }
            //set order total to ordertotal count
            order.Total = orderTotal;
            //save the order
            _StoreDb.SaveChanges();
            //empty the cart
            EmptyCart();
            //return the order id as a confirmation number
            return order.OrderId;
        }
        public string GetCartId(HttpContext context)
        {
            if(context.Session.GetString(CartSessionKey) == null)
            {
                if(!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    //sets the cartId as the username of the user if he's logged in
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    //generate a new id from guid for a guest user
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());

                }
            }
            return context.Session.GetString(CartSessionKey);
        }

        public void SetCartId(HttpContext context)
        {
            //if a guest user logs in we must update the id to the correct id.
            if(!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
        }
        public void MigrateCart(string UserName)
        {
            var shoppingCart = _StoreDb.Carts.Where(
                    c => c.CartId == ShoppingCartId);
            foreach(Cart item in shoppingCart)
            {
                item.CartId = UserName;
            }
            _StoreDb.SaveChanges();
        }
    }
}