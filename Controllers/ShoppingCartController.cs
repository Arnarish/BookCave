using System.Linq;
using System.Web;
using BookCave.Data;
using BookCave.Models.ViewModels;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCave.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Datacontext _StoreDb = new Datacontext();


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
            var addedBook = _StoreDb.Books.SingleOrDefault(book => book.BookId == id);

            //add it to the shopping cart
            var cart = OrderService.GetCart(this.HttpContext);

            cart.AddToCart(addedBook);

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

            return Json(result);
        }
        [HttpPost]
        public IActionResult IncBook(int id)
        {
            var cart = OrderService.GetCart(this.HttpContext);

            //get book from the database
            var addedBook = _StoreDb.Books.SingleOrDefault(book => book.BookId == id);

            cart.AddToCart(addedBook);
            var viewModel = new ShoppingCartViewModel
                {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
                };

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

            return Json(result);
        }
    }
}