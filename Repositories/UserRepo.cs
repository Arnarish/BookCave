using System.Collections.Generic;
using BookCave.Data;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;

namespace BookCave.Repositories
{
    public class UserRepo
    {
        private Datacontext _db;

        public UserRepo()
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