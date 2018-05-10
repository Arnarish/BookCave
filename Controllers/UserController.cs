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
        private UserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
    
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userService = new UserService();
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var user = _userService.GetUserViewModelByString(claim);
            return View(user);
        }
        public IActionResult EditProfile()
        {
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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _userManager.ReplaceClaimAsync(user,
                                                ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "Name"),
                                                new Claim("Name", $"{model.FullName}"));
            var claim = ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value;
            var retUser = _userService.GetUser(claim);
            _userService.UpdateUser(retUser, model);
            return RedirectToAction("Index");
        }

        public IActionResult UserDetails(int? id)
        {
            var user = _userService.GetUserViewModelById(id);
            if(user.Email == ((ClaimsIdentity) User.Identity).FindFirst(c => c.Type == "UserName")?.Value)
            {
                return RedirectToAction("Index");
            }
            if(user == null)
            {
                return View("AccessDenied");
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

            IdentityResult roleResult;
            bool userRoleExists = await _roleManager.RoleExistsAsync("User");
            if (!userRoleExists)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            IdentityResult roleResult1;
            bool AdminRoleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!AdminRoleExists)
            {
                roleResult1 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _userManager.IsInRoleAsync(user, "User"))
            {
                var userResult = await _userManager.AddToRoleAsync(user, "User");
            }
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

                if(!_signInManager.IsSignedIn(User))
                {
                    await _signInManager.SignInAsync(user, false);
                }
                if(model.Image == null)
                {
                    model.Image = "https://www.freeiconspng.com/uploads/profile-icon-9.png";
                }
                _userService.AddUser(model);

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
    }
}