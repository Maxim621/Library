using Library.DAL.Models;
using System;
using System.Collections.Generic;

namespace Library.DAL.Interfaces
{
    public interface IReaderService
    {
        bool Login(string username, string password);
        int GetReaderIdByLogin(string username);

        List<Book> SearchBooksByAuthor(string authorName);
        List<Book> SearchBooksByTitle(string title);
        List<BookLoan> GetBorrowedBooks(int readerId);
        void BorrowBook(int bookId, int readerId);
    }
}
