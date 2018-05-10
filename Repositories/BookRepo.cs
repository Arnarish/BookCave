using System;
using System.Collections.Generic;
using System.Linq;
using BookCave.Data;
using BookCave.Data.EntityModels;
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
        public List<BookListViewModel> GetBooksByRandom()
        {
            var rand = new Random();
            
            var randomizedBooks = (from a in _db.Books
                                    orderby rand.Next()
                                    select new BookListViewModel{
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
                                    }).Take(3).ToList();
                                    
            return randomizedBooks;
        }

        public List<BookListViewModel> GetBooksByAuthor(int? id)
        {
            var book = (from b in _db.Books
                        where b.BookId == id select b).SingleOrDefault();
            var books = (from b in _db.Books
                        where b.Author == book.Author
                        select new BookListViewModel
                        {
                            BookId = b.BookId,
                            Title = b.Title,
                            Author = b.Author,
                            ReleaseYear = b.ReleaseYear,
                            Genre = b.Genre,
                            ISBN = b.ISBN,
                            Price = b.Price,
                            Stock = b.Stock,
                            TopSeller = b.TopSeller,
                            OnSale = b.OnSale,
                            Discount = b.Discount,
                            Image = b.Image,
                        }).ToList();
            return books;
        }

        public List<BookListViewModel> SearchResults(string searchString, string genre)
        {
            if(searchString == null)
            {
                searchString = "";
            }
            if(genre == null)
            {
                genre = "";
            }
            var bookSearch = (from b in _db.Books
                            where (b.ISBN.Contains(searchString) || 
                            b.Title.ToLower().Contains(searchString.ToLower()) || 
                            b.Author.ToLower().Contains(searchString.ToLower())) &&
                            b.Genre.ToLower().Contains(genre.ToLower())
                            orderby b.Title
                            select new BookListViewModel
                            {
                                BookId = b.BookId,
                                Title = b.Title,
                                Author = b.Author,
                                ReleaseYear = b.ReleaseYear,
                                Genre = b.Genre,
                                ISBN = b.ISBN,
                                Price = b.Price,
                                Stock = b.Stock,
                                TopSeller = b.TopSeller,
                                OnSale = b.OnSale,
                                Discount = b.Discount,
                                Image = b.Image,
                            }).ToList();
            return bookSearch;
        }
        public Book GetBookById(int? id)
        {
            var book = (from b in _db.Books
                    where id == b.BookId
                    select b).SingleOrDefault();
                    return book;
        }
        public void AddBook(Book book)
        {
            _db.Add(book);
            _db.SaveChanges();
        }
        public void UpdateBook(Book book)
        {
            _db.Update(book);
            _db.SaveChanges();
        }
        public void RemoveBook(Book book)
        {
            _db.Remove(book);
            _db.SaveChanges();
        }
    }
}