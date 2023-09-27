using Library.DAL.Models;
using System;
using System.Collections.Generic;

namespace Library.DAL.Interfaces
{
    public interface IBookRepository
    {
        Book GetBookById(int bookId);

        List<Book> GetAllBooks();

        List<Book> SearchBooksByTitle(string title);

        List<Book> SearchBooksByAuthor(string authorName);

        List<Book> GetAvailableBooks();

        void AddBook(Book book);

        void UpdateBook(Book book);

        void RemoveBook(int bookId);

        bool CanBorrowBook(int bookId);
    }
}
