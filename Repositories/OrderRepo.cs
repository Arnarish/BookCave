using BookCave.Data;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BookCave.Repositories
{
    public class OrderRepo
    {
        private Datacontext _StoreDb = new Datacontext();
        public OrderRepo()
        {
        }
        public void AddToCart(Cart CartItem)
        {                
            _StoreDb.Carts.Add(CartItem);
            _StoreDb.SaveChanges();
        }
        public void IncBook(Cart CartItem)
        {
            var incItem = _StoreDb.Carts.SingleOrDefault(
                                                 i => i.CartId == CartItem.CartId
                                                && i.BookId == CartItem.BookId);
            incItem.count++;
            _StoreDb.SaveChanges();
        }

        public int DecBook(Cart CartItem)
        {
            var incItem = _StoreDb.Carts.SingleOrDefault(
                                                 i => i.CartId == CartItem.CartId
                                                && i.BookId == CartItem.BookId);
            incItem.count--;
            int count = incItem.count;

            _StoreDb.SaveChanges();

            return count;
        }

        public int RemoveFromCart(Cart CartItem)
        {
            _StoreDb.Remove(CartItem);
            _StoreDb.SaveChanges();

            return (int)0;
        }
            
    }
}