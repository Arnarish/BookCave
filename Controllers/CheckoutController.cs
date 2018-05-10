using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BookCave.Services;
using Microsoft.AspNetCore.Http;
using BookCave.Data.EntityModels;

namespace BookCave.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        CheckoutService _CheckoutS = new CheckoutService();
        OrderService _OrderS = new OrderService();
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(FormCollection values)
        {
            var Order = new Order();
            TryUpdateModelAsync(Order);

            try
            {
                Order.Username = User.Identity.Name;
                Order.OrderDate = DateTime.Now;

                //Add and save order
                _CheckoutS.Add(Order);

                var cart = OrderService.GetCart(this.HttpContext);
                cart.CreateOrder(Order);

                return RedirectToAction("Complete", new { id = Order.OrderId });
            }
            catch
            {
                //invalid, redisplay with errors
                return View(Order);
            }
        }
        public IActionResult Complete(int Id)
        {
            //validate this is the customers order
            bool isValid = _CheckoutS.ValidUserOrder(Id, User.Identity.Name.ToString());

            if(isValid)
            {
                return View(Id);
            }
            else
            {
                return View("Error");
            }
        }
        
    }
}