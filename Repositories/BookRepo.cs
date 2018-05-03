using System.Collections.Generic;
using System.Linq;
using BookCave.Data;
using BookCave.Models.ViewModels;

namespace BookCave.Repositories
{
    public class BookRepo
    {
        private Datacontext _db;

        public BookRepo()
        {
            _db = new Datacontext();
        }
        public List<BookListViewModel> GetAllBooks()
        {
            var books = (from a in _db.Books
                            select new BookListViewModel
                            {
                                BookId = a.BookId,
                                Title = a.Title,
                                Author = a.Author,
                                ReleaseYear = a.ReleaseYear,
                                Genre = a.Genre,
                                ISBN = a.ISBN,
                                Price = a.Price,
                                Stock = a.Stock,
                                TopSeller = a.TopSeller,
                                OnSale = a.OnSale,
                                Discount = a.Discount,
                                Image = a.Image,
                            }).ToList();
            return books;
        }
    }
}