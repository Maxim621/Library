using Library.DAL.Models;
using Library.Interfaces;
using System;
using System.Collections.Generic;

namespace Library.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly List<BookLoan> _bookLoans;
        private readonly List<Reader> _readers;
        private readonly List<Book> _books;

        public BorrowService(List<BookLoan> bookLoans, List<Reader> readers, List<Book> books)
        {
            _bookLoans = bookLoans;
            _readers = readers;
            _books = books;
        }

        public bool CanBorrowBook(int readerId, int bookId)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == readerId);
            var book = _books.FirstOrDefault(b => b.Id == bookId);

            if (reader == null || book == null)
            {
                // Обробка помилки: читача або книгу не знайдено
                return false;
            }

            if (!book.IsAvailable)
            {
                // Обробка помилки: книгу вже позичено
                return false;
            }

            return true;
        }

        public List<BookLoan> GetBorrowedBooks(int readerId)
        {
            return _bookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned == null)
                .ToList();
        }

        public bool BorrowBook(int readerId, int bookId)
        {
            var reader = _readers.FirstOrDefault(r => r.Id == readerId);
            var book = _books.FirstOrDefault(b => b.Id == bookId);

            if (reader == null || book == null)
            {
                // Обробка помилки: читача або книгу не знайдено
                return false;
            }

            if (!book.IsAvailable)
            {
                // Обробка помилки: книгу вже позичено
                return false;
            }

            // Встановлюємо дату повернення (наприклад, через 30 днів)
            DateTime dueDate = DateTime.Now.AddDays(30);

            // Створюємо запис про позичену книгу
            var bookLoan = new BookLoan
            {
                ReaderId = readerId,
                BookId = bookId,
                DueDate = dueDate
            };

            // Додаємо запис про позичену книгу до колекції
            _bookLoans.Add(bookLoan);

            // Позначаємо книгу як позичену
            book.IsAvailable = false;

            return true;
        }

        public bool ReturnBook(int bookLoanId)
        {
            // Знайти запис про позичену книгу за її ідентифікатором
            var bookLoan = _bookLoans.FirstOrDefault(loan => loan.Id == bookLoanId);

            if (bookLoan == null)
            {
                // Позичення не знайдено, повернути помилку
                return false;
            }

            if (bookLoan.DateReturned != null)
            {
                // Книга вже була повернута, повернути помилку
                return false;
            }

            // Встановити дату повернення книги на поточну дату
            bookLoan.DateReturned = DateTime.Now;

            return true;
        }
    }
}
