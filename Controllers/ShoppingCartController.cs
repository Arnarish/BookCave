using System.Linq;
using System.Web;
using BookCave.Data;
using BookCave.Models.ViewModels;
using BookCave.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookCave.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Datacontext _StoreDb = new Datacontext();
        
        //get shopping cart
        public IActionResult index()
        {
            var cart = OrderService.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
              CartItems = cart.GetCartItems(),
              CartTotal = cart.GetTotal()
            };
            //return entire cart
            return View(viewModel);
        }

        public IActionResult AddToCart(int id)
        {
            //get book from the database
            var addedBook = _StoreDb.Books.Single(book => book.BookId == id);

            //add it to the shopping cart
            var cart = OrderService.GetCart(this.HttpContext);

            cart.AddToCart(addedBook);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            //remove the item from the cart
            var cart = OrderService.GetCart(this.HttpContext);
            //get book name for confirmation display
            string bookName = _StoreDb.Carts.Single(book => book.BookId == id).Book.Title;

            //remove from cart
            int itemCount = cart.RemoveFromCart(id);

            //display the confirmation message
            var result = new ShoppingCartRemoveViewModel
            {
                Message =  HttpUtility.HtmlEncode(bookName) + " Has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(result);
        }
        public IActionResult CartSummary()
        {
            var cart = OrderService.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}