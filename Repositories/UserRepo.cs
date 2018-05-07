using System.Collections.Generic;
using BookCave.Data;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services
{
    public class UserService
    {
        private Datacontext _db;

        public UserService()
        {
            _db = new Datacontext();
        }
        /*public UserViewModel GetUser()
        {
            var user = (from 

            return user;
        }*/
        public void AddUser(UserInputModel model)
        {
            _db.Add(model);
        }
    }
}