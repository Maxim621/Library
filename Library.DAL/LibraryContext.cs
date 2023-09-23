using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Library.DAL
{

    public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<PublishingCode> PublishingCodes { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Визначення взаємозв'язку між книгою та автором (багато-до-багатьох)
            modelBuilder.Entity<BookAuthor>()
    .HasKey(ba => new { ba.BookID, ba.AuthorID });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookID);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorID);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookAuthors)
                .WithOne();

            // Визначення зв'язку між читачем і документом (типом документа)
            modelBuilder.Entity<Reader>()
                .HasOne(r => r.Document)
                .WithMany()
                .HasForeignKey(r => r.DocumentId);

            // Визначення взаємозв'язку між позиченнями книг і читачами
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Reader)
                .WithMany(r => r.BookLoans)
                .HasForeignKey(bl => bl.ReaderId);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany()
                .HasForeignKey(bl => bl.BookId);
        }

        // Методи для додавання, оновлення та видалення книг
        public void AddBook(Book newBook)
        {
            Books.Add(newBook);
            SaveChanges();
        }

        public void UpdateBook(Book updatedBook)
        {
            var existingBook = Books.Find(updatedBook.Id);
            if (existingBook != null)
            {
                // Оновлення інформації про книгу
                existingBook.Title = updatedBook.Title;
                existingBook.PublisherCode = updatedBook.PublisherCode;
                SaveChanges();
            }
        }

        public void RemoveBook(int bookId)
        {
            var bookToRemove = Books.Find(bookId);
            if (bookToRemove != null)
            {
                Books.Remove(bookToRemove);
                SaveChanges();
            }
        }

        // Методи для додавання, оновлення та видалення авторів
        public void AddAuthor(Author newAuthor)
        {
            Authors.Add(newAuthor);
            SaveChanges();
        }

        public void AddReader(Reader newReader)
        {
            Readers.Add(newReader);
            SaveChanges();
        }

        public void UpdateAuthor(Author updatedAuthor)
        {
            var existingAuthor = Authors.Find(updatedAuthor.Id);
            if (existingAuthor != null)
            {
                // Оновлення інформації про автора
                existingAuthor.Forename = updatedAuthor.Forename;
                existingAuthor.Surname = updatedAuthor.Surname;
                SaveChanges();
            }
        }

        public void UpdateReader(Reader updatedReader)
        {
            var existingReader = Readers.Find(updatedReader.Id);
            if (existingReader != null)
            {
                // Оновлення інформації про читача
                existingReader.Forename = updatedReader.Forename;
                existingReader.Surname = updatedReader.Surname;
                SaveChanges();
            }
        }

        public void RemoveAuthor(int authorId)
        {
            var authorToRemove = Authors.Find(authorId);
            if (authorToRemove != null)
            {
                Authors.Remove(authorToRemove);
                SaveChanges();
            }
        }

        // Методи для взяття та повернення книги
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
            SaveChanges();
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
            SaveChanges();

            return true;
        }

        // Методи для перевірки статусу книги та історії позичень
        public bool CanBorrowBook(int bookId)
        {
            var book = Books.Find(bookId);

            if (book != null)
            {
                var isBorrowed = BookLoans.Any(loan => loan.BookId == bookId && loan.DateReturned == null);
                return !isBorrowed;
            }

            return false;
        }

        public List<BookLoan> GetLoanHistory(int readerId)
        {
            return BookLoans.Where(loan => loan.ReaderId == readerId).ToList();
        }

        public List<Book> GetAllBooks()
        {
            return Books.ToList();
        }

        public List<Author> GetAllAuthors()
        {
            return Authors.ToList();
        }
        public List<Reader> GetAllReaders()
        {
            return Readers.ToList();
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            return Books.Where(book => book.Title.Contains(title)).ToList();
        }

        public List<Book> SearchBooksByAuthor(string authorName)
        {
            return Books
                .Where(book => book.Authors.Any(author => (author.Forename + " " + author.Surname).Contains(authorName)))
                .ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            return Books.Where(book => book.IsAvailable).ToList();
        }

        public List<Author> SearchAuthorsByName(string authorName)
        {
            return Authors
                .Where(author => (author.Forename + " " + author.Surname).Contains(authorName))
                .ToList();
        }

        public List<BookLoan> GetBorrowedBooks(int readerId)
        {
            return BookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned == null)
                .ToList();
        }

        public List<BookLoan> GetPreviousBorrowedBooks(int readerId)
        {
            return BookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned != null)
                .ToList();
        }

        public List<Reader> GetAllReadersWithHistory()
        {
            var readersWithHistory = new List<Reader>();

            var allReaders = Readers.ToList();

            foreach (var reader in allReaders)
            {
                reader.BookLoans = BookLoans
                    .Where(loan => loan.ReaderId == reader.Id && loan.DateReturned == null)
                    .ToList();

                // Перевірте, чи є боржником
                reader.IsDebtor = reader.BookLoans.Any(loan => loan.DueDate < DateTime.Now);

                readersWithHistory.Add(reader);
            }

            return readersWithHistory;
        }

        public List<BookLoan> GetActiveLoansByReader(int readerId)
        {
            return BookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned == null)
                .ToList();
        }

        public List<BookLoan> GetOverdueLoansByReader(int readerId)
        {
            DateTime currentDate = DateTime.Now;
            return BookLoans
                .Where(loan => loan.ReaderId == readerId && loan.DateReturned < currentDate)
                .ToList();
        }
    }
}
