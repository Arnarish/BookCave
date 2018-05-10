using BookCave.Data.EntityModels;
using BookCave.Repositories;
using System.Web;

namespace BookCave.Services
{
    public class CheckoutService
    {
        CheckoutRepo _CheckRepo = new CheckoutRepo();
        public void Add(Order order)
        {
            _CheckRepo.Add(order);
        }
        public bool ValidUserOrder(int Id, string UserName)
        {
            return _CheckRepo.ValidUserOrder(Id, UserName);            
        }
    }
}