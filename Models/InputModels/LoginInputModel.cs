using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{
    public class LoginInputModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "The password or email was not correct")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}