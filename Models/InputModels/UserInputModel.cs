using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{
    public class UserInputModel
    {
        [Required (ErrorMessage ="Name is required")]
        [MaxLength(28)]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required (ErrorMessage ="Image is required")]
        public string Image { get; set; }
        [Required (ErrorMessage="Address is required")]
        public string Address { get; set; }
        public string Country { get; set; }
        public int Postal { get; set; }
        public int FavoriteBookById { get; set; }
    }
}