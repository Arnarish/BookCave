using System;
using System.Linq;
using System.Web;
using BookCave.Data;
using BookCave.Models.ViewModels;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookCave.Controllers
{
    public class ShoppingCartController : Controller
    {
        private BookService _bookService = new BookService();
        private int CartCount = 0;


        [Authorize]
        public IActionResult Index()
        {
            var cart = OrderService.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
              CartItems = cart.GetCartItems(),
              CartTotal = cart.GetTotal()
            };
            //return entire cart viewModel
            return View(viewModel);
        }
        
        public IActionResult AddToCart(int id)
        {
            //get book from the database
            var addedBook = _bookService.GetBookById(id);
            

            //add it to the shopping cart
            var cart = OrderService.GetCart(this.HttpContext);

            cart.AddToCart(addedBook);

            CartCount++;
            CartCounter(this.HttpContext);

            return RedirectToAction("Index", "Home"); 
        }
        [HttpPost]
        public IActionResult RemoveAll(int id)
        {
            var cart = OrderService.GetCart(this.HttpContext);

            int itemCount = cart.RemoveAllFromCart(id);

            var result = new ShoppingCartRemoveViewModel
            {
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            CartCount = 0;
            CartCounter(this.HttpContext);

            if(result.CartTotal == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return Json(result);
        }
        [HttpPost]
        public IActionResult IncBook(int id)
        {
            var cart = OrderService.GetCart(this.HttpContext);

            //get book from the database
            var addedBook = _bookService.GetBookById(id);

            cart.AddToCart(addedBook);
            var viewModel = new ShoppingCartViewModel
                {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
                };
            CartCount++;
            CartCounter(this.HttpContext);
            if(viewModel.CartTotal == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return Json(viewModel);

        }
        [HttpPost]
        public IActionResult DecBook(int id)
        {
            //remove the item from the cart
            var cart = OrderService.GetCart(this.HttpContext);

            //remove from cart
            int itemCount = cart.RemoveFromCart(id);

            var result = new ShoppingCartRemoveViewModel
            {
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            if(result.CartTotal == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            CartCount--;
            CartCounter(this.HttpContext);
            return Json(result);
        }
        //this function handles the cart counter in the shared layout
        public void CartCounter(HttpContext context)
        {
            var key = context.User.Identity.Name;

            var cart = OrderService.GetCart(this.HttpContext);

            if(CartCount <= 0)
            {
                CartCount = 0;
            }
            //stores the number of items in the cart as int in the session, the session is then read in the _layout view and changed to string.
            context.Session.SetInt32(key, CartCount);
        }
    }
}