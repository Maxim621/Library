using Library.DAL.Models;
using Library.Interfaces;
using Library.DAL;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books;
        private readonly List<Reader> _readers;
        private readonly List<BookLoan> _bookLoans;

        public BookService()
        {
            _books = new List<Book>();
            _readers = new List<Reader>();
            _bookLoans = new List<BookLoan>();
        }

        public Book GetBookById(int bookId)
        {
            return _books.FirstOrDefault(book => book.Id == bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _books.ToList();
        }

        public List<Book> SearchBooksByTitle(string title)
        {
            return _books.Where(book => book.Title.Contains(title)).ToList();
        }

        public List<Book> SearchBooksByAuthor(string authorName)
        {
            return _books
                .Where(book => book.Authors.Any(author => (author.Forename + " " + author.Surname).Contains(authorName)))
                .ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            return _books.Where(book => book.IsAvailable).ToList();
        }

        public void AddBook(Book newBook)
        {
            _books.Add(newBook);
        }

        public void UpdateBook(Book updatedBook)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (existingBook != null)
            {
                // Оновлення книги в колекції
                _books.Remove(existingBook);
                _books.Add(updatedBook);
            }
        }

        public void RemoveBook(int bookId)
        {
            var bookToRemove = _books.FirstOrDefault(b => b.Id == bookId);
            if (bookToRemove != null)
            {
                _books.Remove(bookToRemove);
            }
        }
    }
}