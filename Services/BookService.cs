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
        public BookListViewModel GetBookViewModelById(int? id)
        {
            var book = _bookRepo.GetBookById(id);
            var retBook = new BookListViewModel
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Author = book.Author,
                        ReleaseYear = book.ReleaseYear,
                        Genre = book.Genre,
                        ISBN = book.ISBN,
                        Price = book.Price,
                        Stock = book.Stock,
                        TopSeller = book.TopSeller,
                        OnSale = book.OnSale,
                        Discount = book.Discount,
                        Image = book.Image
                    };
            return retBook;
        }
        public Book GetBookById(int? id)
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
        public void UpdateBook(Book book,BookInputModel model)
        {
            book.Title = model.Title;
            book.Author = model.Author;
            book.ReleaseYear = model.ReleaseYear;
            book.Genre = model.Genre;
            book.ISBN = model.ISBN;
            book.Price = model.Price;
            book.Stock = model.Stock;
            book.TopSeller = model.TopSeller;
            book.OnSale = model.OnSale;
            book.Discount = model.Discount;
            book.Image = model.Image;
            _bookRepo.UpdateBook(book);
        }
    }
}