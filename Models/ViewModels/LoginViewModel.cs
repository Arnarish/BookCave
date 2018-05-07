using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}