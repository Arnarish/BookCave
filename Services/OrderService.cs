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
        private OrderRepo _orderRepo = new OrderRepo();
        //Id to identify the user shopping cart
        string ShoppingCartId { get; set; }
        public const string cartSessionKey = "CartId";
        public static OrderService GetCart(HttpContext context)
        {
            var cart = new OrderService();
            //get a new shopping cart id if the user does not already have one, else returns the given one
            cart.ShoppingCartId = cart.GetCartId(context);
            //returns the cart for the users shopping cart id
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static OrderService GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Book book)
        {
            //add a given book to the cart
            _orderRepo.AddToCart(book, this.ShoppingCartId);
        }
        public int RemoveFromCart(int id)
        {
            //remove book from the cart based on the book id
            return _orderRepo.RemoveFromCart(id, this.ShoppingCartId);
        }
        public int RemoveAllFromCart(int id)
        {
            //removes all books from the cart on a gven bookid
            return _orderRepo.RemoveAllFromCart(id, this.ShoppingCartId);
        }
        public void EmptyCart()
        {
            //empties the cart.
            _orderRepo.EmptyCart(this.ShoppingCartId);
        }
        public List<Cart> GetCartItems()
        {
            //returns all items in the cart
            return _orderRepo.GetCartItems(this.ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            //returns the quantity of items in the cart
            return _orderRepo.GetCount(this.ShoppingCartId);
        }
        public double GetTotal()
        {
            //returns the sum cost of the items in the cart
            return _orderRepo.GetTotal(this.ShoppingCartId);
        }
        public int CreateOrder(Order order)
        {          
            //creates a order from the cart, ready for checkout
            return _orderRepo.CreateOrder(order, this.ShoppingCartId);
        }
        public string GetCartId(HttpContext context)
        {
            if(context.Session.GetString(cartSessionKey) == null)
            {
                if(!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    //sets the cartId as the username of the user if he's logged in
                    context.Session.SetString(cartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    //generate a new id from guid for a guest user
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(cartSessionKey, tempCartId.ToString());

                }
            }
            return context.Session.GetString(cartSessionKey);
        }

        public void SetCartId(HttpContext context)
        {
            //if a guest user logs in we must update the id to the correct id.
            if(!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(cartSessionKey, context.User.Identity.Name);
                }
        }
        public void MigrateCart(string UserName)
        {
            //migrates the cart from a guest user to the username logged in
            _orderRepo.MigrateCart(UserName, this.ShoppingCartId);
        }
        public string GetShoppingCartId()
        {
            //returns the shoppingcart id for this session
            return this.ShoppingCartId;
        }
    }
}