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
                                ReviewScore = a.ReviewScore,
                                TopSeller = a.TopSeller,
                                OnSale = a.OnSale,
                                Discount = a.Discount,
                                Image = a.Image,
                            }).ToList();
            return books;
        }
        public List<BookListViewModel> GetTopTenBooks()
        {
            var topTen = (from a in _db.Books
                          orderby a.ReviewScore descending
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
                                ReviewScore = a.ReviewScore,
                                TopSeller = a.TopSeller,
                                OnSale = a.OnSale,
                                Discount = a.Discount,
                                Image = a.Image,
                          }).Take(10).ToList();
            return topTen;
        }
        public List<BookListViewModel> GetThreeBooksByRandom()
        {
            var rand = new Random();
            
            var randomizedBooks = (from a in _db.Books
                                    where a.Stock != 0
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
                                        ReviewScore = a.ReviewScore,
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
                            ReviewScore = b.ReviewScore,
                            TopSeller = b.TopSeller,
                            OnSale = b.OnSale,
                            Discount = b.Discount,
                            Image = b.Image,
                        }).ToList();
            return books;
        }

        public List<BookListViewModel> SearchResults(string searchString, string genre, int sorted)
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
                                ReviewScore = b.ReviewScore,
                                TopSeller = b.TopSeller,
                                OnSale = b.OnSale,
                                Discount = b.Discount,
                                Image = b.Image,
                            }).ToList();
            if(sorted == 1)
            {
                
            }
            else if(sorted == 2)
            {
                bookSearch = (from b in bookSearch orderby b.Title descending select b).ToList();
            }
            else if(sorted == 3)
            {
                bookSearch = (from b in bookSearch orderby b.Price descending select b).ToList();
            }
            else if(sorted == 4)
            {
                bookSearch = (from b in bookSearch orderby b.Price select b).ToList();
            }
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
            _db.Books.Add(book);
            _db.SaveChanges();
        }
        public void UpdateBook(Book book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
        }
        public void RemoveBook(Book book)
        {
            _db.Books.Remove(book);
            _db.SaveChanges();
        }
    }
}