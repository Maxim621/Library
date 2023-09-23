using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.DAL
{
    public class ReaderService
    {
        private readonly LibraryContext _context;

        public ReaderService()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlServer("Server=DESKTOP-M5BKIQ7\\MSSQLSERVER01;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            _context = new LibraryContext(options);
        }

        public bool Login(string login, string password)
        {
            // Перевірка логіну та пароля читача
            var reader = _context.Readers
                .FirstOrDefault(r => r.Login == login && r.Password == password);

            return reader != null;
        }

        public void RegisterReader(string login, string password, string email, string forename, string surname, int documentId, string documentNumber)
        {
            // Перевірка, чи читач з таким логіном не існує вже в базі даних
            if (_context.Readers.Any(r => r.Login == login))
            {
                Console.WriteLine("Читач з таким логіном вже існує.");
                return;
            }

            // Створення нового об'єкта читача
            var newReader = new Reader
            {
                Login = login,
                Password = password,
                Email = email,
                Forename = forename,
                Surname = surname,
                DocumentId = documentId,
                DocumentNumber = documentNumber
            };

            // Додавання нового читача до бази даних і збереження змін
            _context.Readers.Add(newReader);
            _context.SaveChanges();

            Console.WriteLine("Читач зареєстрований успішно.");
        }

        public bool IsReader(string username)
        {
            return _context.Readers.Any(r => r.Login == username);
        }

        public List<Book> GetAvailableBooks()
        {
            // Повернути список книг, які доступні для взяття
            return _context.Books
                .Where(b => b.IsAvailable)
                .ToList();
        }

        public List<Author> SearchAuthors(string searchTerm)
        {
            // Пошук авторів за заданим критерієм (наприклад, ім'ям чи прізвищем)
            return _context.Authors
                .Where(a => a.Forename.Contains(searchTerm) || a.Surname.Contains(searchTerm))
                .ToList();
        }

        public List<BookLoan> GetBorrowedBooks(int readerId)
        {
            // Отримати список позичених книг для конкретного читача
            return _context.BookLoans
                .Where(bl => bl.ReaderId == readerId)
                .ToList();
        }

        public List<Book> SearchBooksByAuthor(string authorName)
        {
            // Пошук книг за іменем або прізвищем автора
            return _context.Books
                .Where(b => b.Authors.Any(a => a.Forename.Contains(authorName) || a.Surname.Contains(authorName)))
                .ToList();
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            // Пошук книг за назвою
            return _context.Books
                .Where(b => b.Title.Contains(title))
                .ToList();
        }

        public List<Author> SearchAuthorsByName(string authorName)
        {
            // Пошук авторів за іменем або прізвищем
            return _context.Authors
                .Where(a => a.Forename.Contains(authorName) || a.Surname.Contains(authorName))
                .ToList();
        }

        public List<BookLoan> GetOverdueBooks(int readerId)
        {
            // Отримати список прострочених книг для читача
            DateTime currentDate = DateTime.Now;
            return _context.BookLoans
                .Where(bl => bl.ReaderId == readerId && bl.DateReturned < currentDate)
                .OrderByDescending(bl => bl.DateReturned)
                .ToList();
        }

        public int GetReaderIdByLogin(string login)
        {
            var reader = _context.Readers.FirstOrDefault(r => r.Login == login);
            if (reader != null)
            {
                return reader.Id;
            }
            else
            {
                return -1; // Приклад значення за замовчуванням
            }
        }

        public void BorrowBook(int bookId, int readerId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId && b.IsAvailable);

            if (book != null)
            {
                // Створення запису про позику
                var loan = new BookLoan
                {
                    BookId = bookId,
                    ReaderId = readerId,
                    DateBorrowed = DateTime.Now,
                    // Встановлення дати повернення через 30 днів
                    DateReturned = DateTime.Now.AddDays(30)
                };

                // Оновлення статусу книги (вона вже взята)
                book.IsAvailable = false;

                // Додавання запису про позику до контексту та збереження змін в базі даних
                _context.BookLoans.Add(loan);
                _context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Книгу неможливо позичити, оскільки вона вже взята або не існує.");
            }
        }
    }
}
