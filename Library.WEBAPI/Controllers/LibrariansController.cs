using Library.DAL.Models;
using Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Library.WEBAPI.Controllers
{
    [ApiController]
    [Route("api/librarians")]
    public class LibrariansController : ControllerBase
    {
        private readonly ILibrarianService _librarianService;

        public LibrariansController(ILibrarianService librarianService)
        {
            _librarianService = librarianService;
        }

        [HttpGet("all-books")]
        public ActionResult<List<Book>> GetAllBooks()
        {
            var books = _librarianService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("all-authors")]
        public ActionResult<List<Author>> GetAllAuthors()
        {
            var authors = _librarianService.GetAllAuthors();
            return Ok(authors);
        }

        [HttpPost("add-or-update-book")]
        public ActionResult AddOrUpdateBook([FromBody] Book book)
        {
            _librarianService.AddOrUpdateBook(book);
            return Ok();
        }

        [HttpPost("add-or-update-author")]
        public ActionResult AddOrUpdateAuthor([FromBody] Author author)
        {
            _librarianService.AddOrUpdateAuthor(author);
            return Ok();
        }

        [HttpDelete("delete-reader/{readerId}")]
        public ActionResult DeleteReader(int readerId)
        {
            _librarianService.DeleteReader(readerId);
            return Ok();
        }

        [HttpGet("debtors")]
        public ActionResult<List<Reader>> GetDebtors()
        {
            var debtors = _librarianService.GetDebtors();
            return Ok(debtors);
        }

        [HttpGet("all-readers")]
        public ActionResult<List<Reader>> GetAllReaders()
        {
            var readers = _librarianService.GetAllReaders();
            return Ok(readers);
        }

        [HttpGet("borrow-history/{readerId}")]
        public ActionResult<List<BookLoan>> GetBorrowHistory(int readerId)
        {
            var borrowHistory = _librarianService.GetBorrowHistory(readerId);
            return Ok(borrowHistory);
        }
    }
}