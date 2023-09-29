using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Interfaces;
using Library.DAL.Models;
using Library.WEBAPI.Dtos;

namespace Library.WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetAllBooks()
        {
            var books = _bookService.GetAllBooks();
            // Мапуємо об'єкти Book на BookDto, якщо потрібно
            // Повертаємо список книг у вигляді DTO
            return Ok(books);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDto> GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound(); // Повертаємо 404 Not Found, якщо книга не знайдена
            }
            // Мапуємо об'єкт Book на BookDto, якщо потрібно
            // Повертаємо книгу у вигляді DTO
            return Ok(book);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(); // Повертаємо 400 Bad Request, якщо дані недійсні
            }

            // Мапуємо BookDto на Book (якщо у вас є маппер або якщо це робиться вручну)
            var book = new Book
            {
                // Присвоюємо властності вашого об'єкта Book на основі даних з BookDto
                // Наприклад:
                Title = bookDto.Title,
                Author = bookDto.Author,
                // Інші властності
            };

            // Додаємо книгу
            _bookService.AddBook(book);

            // Опціонально: Можливо, ви хочете повернути створену книгу назад як відповідь
            // Можете використовувати маппер для цього
            var createdBookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                // Інші властності
            };

            return CreatedAtAction(nameof(GetBookById), new { id = createdBookDto.Id }, createdBookDto); // Повертаємо 201 Created
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookDto updatedBookDto)
        {
            if (updatedBookDto == null)
            {
                return BadRequest("Дані книги для оновлення не надані.");
            }

            // Конвертуємо DTO в об'єкт Book
            var updatedBook = new Book
            {
                Id = id,
                Title = updatedBookDto.Title,
                // Інші властивості, які вам потрібно оновити
            };

            // Викликаємо метод сервісу для оновлення книги
            _bookService.UpdateBook(id, updatedBook);

            return Ok("Книга оновлена успішно.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound(); // Повертаємо 404 Not Found, якщо книга не знайдена
            }
            _bookService.RemoveBook(id);
            return NoContent(); // Повертаємо 204 No Content
        }
    }
}