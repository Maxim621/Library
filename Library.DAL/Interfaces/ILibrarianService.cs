using Library.DAL.Models;
using System;
using System.Collections.Generic;

namespace Library.DAL.Interfaces
{
    public interface ILibrarianService
    {
        bool Login(string login, string password);
        List<Book> GetAllBooks();
        void DeleteReader(int readerId);
        List<Reader> GetDebtors();
        List<Reader> GetAllReaders();
        List<BookLoan> GetBorrowHistory(int readerId);
        void AddOrUpdateBook(Book book);
        void AddOrUpdateAuthor(Author author);
    }
}
