using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{
    public class LoginInputModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is valid")]
        public string Email { get; set; }
        
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}