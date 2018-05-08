using System.Collections.Generic;
using BookCave.Data;
using System.Linq;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using System;

namespace BookCave.Repositories
{
    public class UserRepo
    {
        private Datacontext _db;

        public UserRepo()
        {
            _db = new Datacontext();
        }
        public UserViewModel GetUserViewModel(string user)
        {
            var retUser = (from u in _db.Users
                    where u.Email == user
                    select new UserViewModel
                    {
                        UserId = u.UserId,
                        Email = u.Email,
                        FullName = u.FullName,
                        Image = u.Image,
                        Address = u.Address,
                        Country = u.Country,
                        Postal = u.Postal,
                        FavoriteBookById = u.FavoriteBookById
                    }).SingleOrDefault();    

                    return retUser;      
        }
        public User GetUser(string user)
        {
            var retUser = (from u in _db.Users
                    where u.Email == user
                    select u).SingleOrDefault();
                    return retUser;
        }
        public void AddUser(User model)
        {
            _db.Users.Add(model);
            _db.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }
    }
}