using System.Collections.Generic;
using BookCave.Data.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using BookCave.Repositories;

namespace BookCave.Services
{
    public class BookService
    {
        private BookRepo _bookRepo;

        public BookService()
        {
            _bookRepo = new BookRepo();
        }
        public List<BookListViewModel> GetAllBooks()
        {
            var books = _bookRepo.GetAllBooks();

            return books;
        }
        public List<BookListViewModel> GetBooksByAuthor(int? id)
        {
            return _bookRepo.GetBooksByAuthor(id);
        }
        public List<BookListViewModel> GetThreeBooksByRandom()
        {
            var randomizedBooks = _bookRepo.GetThreeBooksByRandom();
            
            return randomizedBooks;
        }

        public List<BookListViewModel> SearchResults(string searchString, string genre)
        {
            return _bookRepo.SearchResults(searchString, genre);
        }
        public BookListViewModel GetBookById(int? id)
        {
            return _bookRepo.GetBookById(id);
        }
        public void AddBook(BookInputModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                ReleaseYear = model.ReleaseYear,
                Genre = model.Genre,
                ISBN = model.ISBN,
                Price = model.Price,
                Stock = model.Stock,
                TopSeller = model.TopSeller,
                OnSale = model.OnSale,
                Discount = model.Discount,
                Image = model.Image,
            };
            _bookRepo.AddBook(book);
        }
        public void UpdateBook(BookInputModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                ReleaseYear = model.ReleaseYear,
                Genre = model.Genre,
                ISBN = model.ISBN,
                Price = model.Price,
                Stock = model.Stock,
                TopSeller = model.TopSeller,
                OnSale = model.OnSale,
                Discount = model.Discount,
                Image = model.Image,
            };
            _bookRepo.UpdateBook(book);
        }
    }
}