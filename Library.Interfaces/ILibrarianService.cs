using System;
using Library.DAL.Models;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface ILibrarianService
    {
        bool Login(string login, string password);
        List<Book> GetAllBooks();
        void DeleteReader(int readerId);
        List<Reader> GetDebtors();
        List<Author> GetAllAuthors();
        List<Reader> GetAllReaders();
        List<BookLoan> GetBorrowHistory(int readerId);
        void AddOrUpdateBook(Book book);
        void AddOrUpdateAuthor(Author author);
    }
}
