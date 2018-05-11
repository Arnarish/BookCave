using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BookCave.Services;
using Microsoft.AspNetCore.Http;
using BookCave.Data.EntityModels;
using BookCave.Models.ViewModels;
using System.Security.Claims;
using BookCave.Models.InputModels;

namespace BookCave.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        private OrderService _orderS = new OrderService();
        private CheckoutService _checkoutS = new CheckoutService();
        private UserService _userS = new UserService();
        public IActionResult Checkout()
        {
            var cart = OrderService.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
              CartItems = cart.GetCartItems(),
              CartTotal = cart.GetTotal()
            };
            if(viewModel.CartTotal <= 0)
            {
                return View("Index", "Home");
            }
            
            return View(viewModel);
        }
        public IActionResult Billing()
        {
            var User = GetUser();
            var CheckoutView = new CheckoutUserViewModel()
            {
                FullName = User.FullName,
                Address = User.Address,
                PostalCode = User.Postal.ToString(),
                Email = User.Email
            };
            return View(CheckoutView);
        }
        [HttpPost]
        public IActionResult Checkout(OrderInputModel OrderModel)
        {
            var Order = new Order();
            var cart = OrderService.GetCart(this.HttpContext);
            

            try
            {
                // fill out the order with the data from the user
                Order.Username = User.Identity.Name;
                Order.OrderDate = DateTime.Now;
                Order.FullName = OrderModel.FullName;
                Order.Address = OrderModel.Address;
                Order.City = OrderModel.City;
                Order.PostalCode = OrderModel.PostalCode;
                Order.Country = OrderModel.Country;
                Order.Email = OrderModel.Email;
                Order.Total = _orderS.GetTotal();

                //Add and save order
                _checkoutS.Add(Order);

                int OrderId = cart.CreateOrder(Order);


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
            bool isValid = _checkoutS.ValidUserOrder(Id, User.Identity.Name.ToString());

            if(isValid)
            {
                return View(Id);
            }
            else
            {
                return View("Error");
            }
        }

        private UserViewModel GetUser()
        {
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var CurrentUser = _userS.GetUserViewModelByString(claim);

            return CurrentUser;
        }
        
    }
}