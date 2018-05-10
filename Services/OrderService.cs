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
                _OrderRep.AddToCart(book, this.ShoppingCartId);
        }
        public int RemoveFromCart(int id)
        {
            return _OrderRep.RemoveFromCart(id, this.ShoppingCartId);
        }
        public void EmptyCart()
        {
            _OrderRep.EmptyCart(this.ShoppingCartId);
        }
        public List<Cart> GetCartItems()
        {
            return _OrderRep.GetCartItems(this.ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            return _OrderRep.GetCount(this.ShoppingCartId);
        }
        public double GetTotal()
        {
            return _OrderRep.GetTotal(this.ShoppingCartId);
        }
        public int CreateOrder(Order order)
        {          
            return _OrderRep.CreateOrder(order, this.ShoppingCartId);
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
            _OrderRep.MigrateCart(UserName, this.ShoppingCartId);
        }
        public string GetShoppingCartId()
        {
            return this.ShoppingCartId;
        }
    }
}