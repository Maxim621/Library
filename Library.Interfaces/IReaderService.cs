using System;
using Library.DAL.Models;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IReaderService
    {
        bool Login(string login, string password);
        bool IsReader(string username);
        List<Book> GetAvailableBooks();
        List<Author> SearchAuthors(string searchTerm);
        List<BookLoan> GetBorrowedBooks(int readerId);
        List<Book> SearchBooksByAuthor(string authorName);
        List<Book> SearchBooksByTitle(string title);
        List<Author> SearchAuthorsByName(string authorName);
        List<BookLoan> GetOverdueBooks(int readerId);
        int GetReaderIdByLogin(string login);
        void AddReader(Reader newReader);
        void UpdateReader(Reader updatedReader);
        List<Reader> GetAllReaders();
        void BorrowBook(int bookId, int readerId);
        List<BookLoan> GetActiveLoansByReader(int readerId);
        List<BookLoan> GetOverdueLoansByReader(int readerId);
        List<Reader> GetAllReadersWithHistory();
    }
}
