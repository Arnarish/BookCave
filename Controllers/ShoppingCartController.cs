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
        //CartCount used to keep track of how many items are in the cart while user is not in the cart itself.
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
            
            var cart = OrderService.GetCart(this.HttpContext);
            //add it to the shopping cart
            cart.AddToCart(addedBook);

            CartCount++;
            CartCounter(this.HttpContext);
            //redirect the user to the frontpage.
            return RedirectToAction("Index", "Home"); 
        }
        [HttpPost]
        public IActionResult RemoveAll(int id)
        {
            var cart = OrderService.GetCart(this.HttpContext);
            //removes all items with a given id from the cart
            int itemCount = cart.RemoveAllFromCart(id);
            //viewmodel used to update site via Ajax
            var result = new ShoppingCartRemoveViewModel
            {
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            //reset the CartCount, then call counter to update the _layout
            CartCount = 0;
            CartCounter(this.HttpContext);

            return Json(result);            
        }
        [HttpPost]
        public IActionResult IncBook(int id)
        {
            /*function used to increment count in cart on a given book id*/
            var cart = OrderService.GetCart(this.HttpContext);

            //get book from the database
            var addedBook = _bookService.GetBookById(id);
            //increase the count of the book in the cart
            cart.AddToCart(addedBook);
            //update viewmodel, Ajax
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
            else
            {
                return Json(viewModel);
            }
            

        }
        [HttpPost]
        public IActionResult DecBook(int id)
        {
            /* function used to decrement book quantity based on a given book id, if the quantity becomes 0 it removes the book. */
            var cart = OrderService.GetCart(this.HttpContext);

            //remove from cart
            int itemCount = cart.RemoveFromCart(id);
            //SCRVM used for AJAX
            var result = new ShoppingCartRemoveViewModel
            {
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            CartCount--;
            CartCounter(this.HttpContext);

            if(result.CartTotal == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Json(result);
            }
            
            
        }
        //this function handles the cart counter in the shared layout
        public void CartCounter(HttpContext context)
        {
            string key = context.Session.Id.ToString();

            if(CartCount <= 0)
            {
                CartCount = 0;
            }
            //stores the number of items in the cart as int in the session, the session is then read in the _layout view and changed to string.
            context.Session.SetInt32(key, CartCount);
        }
    }
}