using System;
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
        public List<BookListViewModel> GetTopTenBooks()
        {
            var books =  _bookRepo.GetTopTenBooks();
            
            foreach(var b in books)
            {
                if(b.OnSale)
                {
                    b.Price = Math.Round(b.Price - b.Price*((double)b.Discount/100), 2);
                }
            
                b.ReviewScore = Math.Round(b.ReviewScore, 1);
            }
            return books;
        }
        public List<BookListViewModel> GetBooksByAuthor(int? id)
        {
            return _bookRepo.GetBooksByAuthor(id);
        }
        public List<BookListViewModel> GetBooksByRandom()
        {
            var randomizedBooks = _bookRepo.GetThreeBooksByRandom();

            foreach(var b in randomizedBooks)
            {
                if(b.OnSale)
                {
                    b.Price = Math.Round(b.Price - b.Price*((double)b.Discount/100), 2);
                }
            }
            return randomizedBooks;
        }

        public List<BookListViewModel> SearchResults(string searchString, string genre, int sorted)
        {
            
            var books =  _bookRepo.SearchResults(searchString, genre, sorted);
            foreach(var b in books)
            {
                if(b.OnSale)
                {
                    b.Price = Math.Round(b.Price - b.Price*((double)b.Discount/100), 2);
                }
            }
            return books;
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
            var book = _bookRepo.GetBookById(id);
                if(book.OnSale)
                {
                    book.Price = Math.Round(book.Price - book.Price*((double)book.Discount/100), 2);
                }
            return book;
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
        public void RemoveBook(Book book)
        {
            _bookRepo.RemoveBook(book);
        }
    }
}