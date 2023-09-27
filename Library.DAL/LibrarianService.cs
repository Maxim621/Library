using Library.DAL.Interfaces;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Library.DAL
{
    public class LibrarianService : ILibrarianService
    {
        public DbSet<Author> Authors { get; set; }

        private readonly LibraryContext _context;

        public LibrarianService(DbContextOptions<LibraryContext> options)
        {
            _context = new LibraryContext(options);
        }

        public bool Login(string login, string password)
        {
            // Перевірка логіну та пароля бібліотекаря
            var librarian = _context.Librarians
                .FirstOrDefault(b => b.Login == login && b.Password == password);

            return librarian != null;
        }

        public void AddAuthor(Author newAuthor)
        {
            Authors.Add(newAuthor);
            _context.SaveChanges();
        }
        public void RegisterLibrarian(string login, string password, string email)
        {
            // Перевірка, чи бібліотекар з таким логіном не існує вже в базі даних
            if (_context.Librarians.Any(l => l.Login == login))
            {
                Console.WriteLine("Бібліотекар з таким логіном вже існує.");
                return;
            }

            // Створення нового об'єкта бібліотекаря
            var newLibrarian = new Librarian
            {
                Login = login,
                Password = password,
                Email = email
            };

            // Додавання нового бібліотекаря до бази даних і збереження змін
            _context.Librarians.Add(newLibrarian);
            _context.SaveChanges();

            Console.WriteLine("Бібліотекар зареєстрований успішно.");
        }
        public void UpdateAuthor(Author updatedAuthor)
        {
            var existingAuthor = Authors.Find(updatedAuthor.Id);
            if (existingAuthor != null)
            {
                // Оновлення інформації про автора
                existingAuthor.Forename = updatedAuthor.Forename;
                existingAuthor.Surname = updatedAuthor.Surname;
                _context.SaveChanges();
            }
        }

        

        public List<Author> GetAllAuthors()
        {
            return Authors.ToList();
        }

        public void RemoveAuthor(int authorId)
        {
            var authorToRemove = Authors.Find(authorId);
            if (authorToRemove != null)
            {
                Authors.Remove(authorToRemove);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            // Повернути список усіх книг
            return _context.Books.ToList();
        }

        public List<Author> SearchAuthorsByName(string authorName)
        {
            return Authors
                .Where(author => (author.Forename + " " + author.Surname).Contains(authorName))
                .ToList();
        }

        public void AddOrUpdateBook(Book book)
        {
            // Додати нову книгу або оновити існуючу
            if (book.Id == 0)
            {
                _context.Books.Add(book); // Додавання нової книги
            }
            else
            {
                _context.Books.Update(book); // Оновлення існуючої книги
            }
            _context.SaveChanges();
        }

        public void AddOrUpdateAuthor(Author author)
        {
            // Додати нового автора або оновити існуючого
            if (author.Id == 0)
            {
                _context.Authors.Add(author); // Додавання нового автора
            }
            else
            {
                _context.Authors.Update(author); // Оновлення існуючого автора
            }
            _context.SaveChanges();
        }

        public void DeleteReader(int readerId)
        {
            // Видалити читача за ідентифікатором
            var reader = _context.Readers.FirstOrDefault(r => r.Id == readerId);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
        }
        public List<Reader> GetAllReaders()
        {
            // Повернути список усіх читачів
            return _context.Readers.ToList();
        }

        public List<Reader> GetDebtors()
        {
            // Отримати список боржників (читачів, які мають неповернені книги)
            var currentDate = DateTime.Now;
            return _context.Readers
                .Where(r => r.BookLoans.Any(bl => bl.DateReturned < currentDate))
                .ToList();
        }

        public List<BookLoan> GetBorrowHistory(int readerId)
        {
            // Отримати історію взяття та повернення книг для конкретного читача
            return _context.BookLoans
                .Where(bl => bl.ReaderId == readerId)
                .ToList();
        }
    }
}
