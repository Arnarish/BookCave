using System.Collections.Generic;
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

    }
}