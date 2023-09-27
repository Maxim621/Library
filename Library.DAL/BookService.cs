using Library.DAL.Interfaces;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library.DAL
{
    public class BookService : IBookService
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Reader> Readers { get; set; }

        public DbSet<BookLoan> BookLoans { get; set; }

        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.FirstOrDefault(book => book.Id == bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            return _context.Books.Where(book => book.Title.Contains(title)).ToList();
        }

        public List<Book> SearchBooksByAuthor(string authorName)
        {
            return _context.Books
                .Where(book => book.Authors.Any(author => (author.Forename + " " + author.Surname).Contains(authorName)))
                .ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            return _context.Books.Where(book => book.IsAvailable).ToList();
        }

        public bool CanBorrowBook(int bookId)
        {
            var book = _context.Books.Find(bookId);

            if (book != null)
            {
                var isBorrowed = _context.BookLoans.Any(loan => loan.BookId == bookId && loan.DateReturned == null);
                return !isBorrowed;
            }

            return false;
        }

        public List<BookLoan> GetBorrowedBooks(int readerId)
        {
            return BookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned == null)
                .ToList();
        }

        public void BorrowBook(int bookId, int readerId)
        {
            var reader = Readers.FirstOrDefault(r => r.Id == readerId);
            var book = Books.FirstOrDefault(b => b.Id == bookId);

            if (reader == null || book == null)
            {
                // Обробка помилки: читача або книгу не знайдено
                return;
            }

            // Перевірка, чи книга доступна для позичення
            if (!book.IsAvailable)
            {
                // Обробка помилки: книгу вже позичено
                return;
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

            // Додаємо запис про позичену книгу до бази даних
            BookLoans.Add(bookLoan);

            // Позначаємо книгу як позичену
            book.IsAvailable = false;

            // Зберігаємо зміни в базу даних
            _context.SaveChanges();
        }

        public bool ReturnBook(int bookLoanId)
        {
            // Знайти запис про позичену книгу за її ідентифікатором
            var bookLoan = BookLoans.FirstOrDefault(loan => loan.Id == bookLoanId);

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

            // Зберегти зміни у базі даних
            _context.SaveChanges();

            return true;
        }

        public void AddBook(Book newBook)
        {
            _context.Books.Add(newBook);
            _context.SaveChanges();
        }

        public void UpdateBook(Book updatedBook)
        {
            // Оновлення книги в контексті
            _context.Entry(updatedBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoveBook(int bookId)
        {
            var bookToRemove = _context.Books.Find(bookId);
            if (bookToRemove != null)
            {
                _context.Books.Remove(bookToRemove);
                _context.SaveChanges();
            }
        }
    }
}
