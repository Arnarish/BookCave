using BookCave.Data;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookCave.Services
{
    public partial class OrderService
    {
        private Datacontext _StoreDb = new Datacontext();
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
                    BookId = book.BookId,
                    CartId = ShoppingCartId,
                    count = 1,
                    DateCreated = DateTime.Now
                };
                _StoreDb.Carts.Add(CartItem);
            }
            else
            {
                //item exists in cart, increase quantity
                CartItem.count++;
            }
            _StoreDb.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            //get the cart
            var cartitem = _StoreDb.Carts.Single(
                            cart => cart.CartId == ShoppingCartId
                            && cart.BookId == id);
            int itemcount = 0;
            if(cartitem != null)
            {
                if(cartitem.count > 1)
                {
                    cartitem.count--;
                    itemcount = cartitem.count;
                }
                else
                {
                    _StoreDb.Remove(cartitem);
                }
                _StoreDb.SaveChanges();
            }
            return itemcount;
        }
        public void EmptyCart()
        {
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
            return _StoreDb.Carts.Where(
                        cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            int? count = (from cartItems in _StoreDb.Carts
                        where cartItems.CartId == ShoppingCartId
                        select (int?)cartItems.count).Sum();
            //return 0 if all entries are null
            return count ?? 0;
        }
        public double GetTotal()
        {
            double? total = (from cartItems in _StoreDb.Carts
                                where cartItems.CartId == ShoppingCartId
                                select (int?)cartItems.count * cartItems.Book.Price).Sum();

            return total ?? 0;
        }
        public int CreateOrder(Order order)
        {
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
                    //context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    //generate a new id from guid
                    Guid tempCartId = Guid.NewGuid();
                    //context.Session.SetString(CartSessionKey, tempCartId.ToString());
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());

                }
            }
            return context.Session.GetString(CartSessionKey);
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