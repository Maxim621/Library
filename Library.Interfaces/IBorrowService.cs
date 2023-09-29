using System.Collections.Generic;
using Library.DAL.Models;

namespace Library.Interfaces
{
    public interface IBorrowService
    {
        bool CanBorrowBook(int readerId, int bookId);
        List<BookLoan> GetBorrowedBooks(int readerId);
        bool BorrowBook(int readerId, int bookId);
        bool ReturnBook(int bookLoanId);
    }
}
