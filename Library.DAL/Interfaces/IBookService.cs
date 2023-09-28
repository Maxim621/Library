using Library.DAL.Models;
using System;
using System.Collections.Generic;

namespace Library.DAL.Interfaces
{
    public interface IBookService
    {
        Book GetBookById(int bookId);
        List<Book> GetAllBooks();
        List<Book> SearchBooksByTitle(string title);
        List<Book> SearchBooksByAuthor(string authorName);
        List<Book> GetAvailableBooks();
        bool CanBorrowBook(int bookId);
        List<BookLoan> GetBorrowedBooks(int readerId);
        void BorrowBook(int bookId, int readerId);
        bool ReturnBook(int bookLoanId);
        void AddBook(Book newBook);
        void UpdateBook(Book updatedBook);
        void RemoveBook(int bookId);
    }
}
