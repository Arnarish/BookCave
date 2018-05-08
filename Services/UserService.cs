using System.Collections.Generic;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services
{
    public class UserService
    {
        private UserRepo _userRepo;

        public UserService()
        {
            _userRepo = new UserRepo();
        }
        public UserViewModel GetUserViewModel(string user)
        {
            return _userRepo.GetUserViewModel(user);
        }
        public User GetUser(string user)
        {
            return _userRepo.GetUser(user);
        }
        public void AddUser(RegisterInputModel model)
        {
            var user = new User
            {
                Email = model.Email,
                FullName = model.FirstName + " " + model.LastName,
                Image = model.Image,
                Address = model.Address,
                Country = model.Country,
                Postal = model.Postal,
                FavoriteBookById = 0
            };
            _userRepo.AddUser(user);
        }
        public void UpdateUser(User user, UserInputModel model)
        {
            user.FullName = model.FullName;
            user.Address = model.Address;
            user.Image = model.Image;
            user.Country = model.Country;
            user.Postal = model.Postal;
            _userRepo.UpdateUser(user);
        }
    }
}