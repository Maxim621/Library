using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.DAL.Models;
using Library.Interfaces;
using Library.WEBAPI.Dtos;

namespace Library.WEBAPI.Controllers
{
    [Route("api/borrow")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpGet("borrowed-books/{readerId}")]
        public IActionResult GetBorrowedBooks(int readerId)
        {
            var borrowedBooks = _borrowService.GetBorrowedBooks(readerId);
            var borrowedBooksDto = new List<BookLoanDto>();

            foreach (var bookLoan in borrowedBooks)
            {
                var bookLoanDto = new BookLoanDto
                {
                    Id = bookLoan.Id,
                    BookId = bookLoan.BookId,
                    DueDate = bookLoan.DueDate
                    // Додайте інші властивості, які вам потрібні
                };
                borrowedBooksDto.Add(bookLoanDto);
            }

            return Ok(borrowedBooksDto);
        }

        [HttpPost("borrow-book")]
        public IActionResult BorrowBook([FromBody] BorrowBookDto borrowBookDto)
        {
            if (borrowBookDto == null)
            {
                return BadRequest("Дані для позики книги не надані.");
            }

            bool result = _borrowService.BorrowBook(borrowBookDto.ReaderId, borrowBookDto.BookId);

            if (result)
            {
                return Ok("Книгу успішно позичено.");
            }
            else
            {
                return BadRequest("Помилка позики книги.");
            }
        }

        [HttpPost("return-book/{bookLoanId}")]
        public IActionResult ReturnBook(int bookLoanId)
        {
            bool result = _borrowService.ReturnBook(bookLoanId);

            if (result)
            {
                return Ok("Книгу успішно повернено.");
            }
            else
            {
                return BadRequest("Помилка повернення книги.");
            }
        }
    }
}