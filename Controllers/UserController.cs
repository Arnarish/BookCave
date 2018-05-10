using System.Security.Claims;
using System.Threading.Tasks;
using BookCave.Models;
using BookCave.Models.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookCave.Services;

namespace BookCave.Controllers
{
    public class UserController : Controller
    {
        private ReviewService _reviewService;
        private UserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
    
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _reviewService = new ReviewService();
            _userService = new UserService();
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult EditProfile()
        {
            //Use claim to reach the username easier, then get the user profile with the user name
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var user = _userService.GetUserViewModelByString(claim);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserInputModel model)
        {
            
            if(!ModelState.IsValid)
            {
                return View();
            }
            //Get The ApplicationUser to be able to replace the claim of the name that appears in the loginPartialView since it has been edited
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _userManager.ReplaceClaimAsync(user,
                                                ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "Name"),
                                                new Claim("Name", $"{model.FullName}"));
            //Get the user profile using the claim then get the user with the get function from the userService finally update the user
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var retUser = _userService.GetUser(claim);
            _userService.UpdateUser(retUser, model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ChangeFavoriteBook(int id)
        {
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var user = _userService.GetUser(claim);
            _userService.ChangeFavoriteBook(user, id);
            return Ok();
        }
        [Authorize]
        public IActionResult Index()
        {
            //this is the currently logged in users profile
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var user = _userService.GetUserViewModelByString(claim);
            user = _reviewService.AddReviewsToViewModel(user);
            return View(user);
        }
        public IActionResult UserDetails(int? id)
        {
            //this is other users
            var user = _userService.GetUserViewModelById(id);
            if(user == null)
            {
                return View("NotFound");
            }
            user = _reviewService.AddReviewsToViewModel(user);
            //if the user tries to access his profile through otherUsers he will be redirected to his profile that is on the action Index
            if(user.Email == ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            //adding the role User if it does not exist
            IdentityResult roleResult;
            bool userRoleExists = await _roleManager.RoleExistsAsync("User");
            if (!userRoleExists)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            //adding the role Admin if it does not exist
            IdentityResult roleResult1;
            bool AdminRoleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!AdminRoleExists)
            {
                roleResult1 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //if the current user that is registering does not have the role has user he is given that role
            if (!await _userManager.IsInRoleAsync(user, "User"))
            {
                var userResult = await _userManager.AddToRoleAsync(user, "User");
            }
            //if in the admin register view the variable employee is checked the registered user is granted the role admin
            if (model.Admin)
            {
                var userResult = await _userManager.AddToRoleAsync(user, "Admin");
            }
            if(result.Succeeded)
            {
                //The user is Successfully registered
                // Add the Concatenated first and last name as fullname in claims
                await _userManager.AddClaimAsync(user, new Claim("Name", $"{model.FirstName} {model.LastName}"));
                await _userManager.AddClaimAsync(user, new Claim("UserName", $"{model.Email}"));

                //if the user is already signed in he is an admin creating another account so he wont be locked in to the new account
                if(!_signInManager.IsSignedIn(User))
                {
                    await _signInManager.SignInAsync(user, false);
                }
                //if the user skipped the image he is given a default image
                if(model.Image == null)
                {
                    model.Image = "https://www.freeiconspng.com/uploads/profile-icon-9.png";
                }
                _userService.AddUser(model);
                MigrateShoppingCart(model.Email.ToString());

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Login()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if(result.Succeeded)
            {
                MigrateShoppingCart(model.Email.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private void MigrateShoppingCart(string UserName)
        {
            var cart = OrderService.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            this.HttpContext.Session.Clear();
            cart.SetCartId(this.HttpContext);
            
        }
    }
}