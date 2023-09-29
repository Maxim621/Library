using Microsoft.AspNetCore.Mvc;
using Library.WEBAPI.Dtos;
using Library.Interfaces;
using System;
using System.Collections.Generic;

namespace Library.Controllers
{
    [Route("api/readers")]
    [ApiController]
    public class ReadersController : ControllerBase
    {
        private readonly IReaderService _readerService;

        public ReadersController(IReaderService readerService)
        {
            _readerService = readerService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            // Виклик сервісу для автентифікації
            bool isAuthenticated = _readerService.Login(model.Username, model.Password);

            if (isAuthenticated)
            {
                // Повернення успіху або JWT токена, якщо потрібно
                return Ok(new { message = "Автентифікація пройшла успішно" });
            }

            return Unauthorized(new { message = "Невірний логін або пароль" });
        }

        [HttpPost("register")]
        public IActionResult RegisterReader([FromBody] RegisterReaderDto model)
        {
            // Виклик сервісу для реєстрації читача
            _readerService.RegisterReader(model.Login, model.Password, model.Email, model.Forename, model.Surname, model.DocumentId, model.DocumentNumber);

            // Повернення успішної відповіді
            return Ok(new { message = "Читач зареєстрований успішно" });
        }

        [HttpGet("available-books")]
        public IActionResult GetAvailableBooks()
        {
            // Отримання доступних книг
            var availableBooks = _readerService.GetAvailableBooks();
            return Ok(availableBooks);
        }

        // Додайте інші методи контролера тут для інших операцій
    }
}