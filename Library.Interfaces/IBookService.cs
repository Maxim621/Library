using System;
using Library.DAL.Models;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IBookService
    {
        Book GetBookById(int bookId);
        List<Book> GetAllBooks();
        List<Book> SearchBooksByTitle(string title);
        List<Book> SearchBooksByAuthor(string authorName);
        List<Book> GetAvailableBooks();
        void AddBook(Book newBook);
        void UpdateBook(Book updatedBook);
        void RemoveBook(int bookId);
    }
}
