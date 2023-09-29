using Microsoft.AspNetCore.Mvc;
using Library.Interfaces;
using Library.WEBAPI.Dtos;

namespace Library.WEBAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            // Виклик сервісу для автентифікації
            bool isAuthenticated = _authService.Login(model.Username, model.Password);

            if (isAuthenticated)
            {
                // Повернення успіху або JWT токена, якщо потрібно
                return Ok(new { message = "Автентифікація пройшла успішно" });
            }

            return Unauthorized(new { message = "Невірний логін або пароль" });
        }

        [HttpPost("register/librarian")]
        public IActionResult RegisterLibrarian([FromBody] RegisterLibrarianDto model)
        {
            // Виклик сервісу для реєстрації бібліотекаря
            _authService.RegisterLibrarian(model.Login, model.Password, model.Email);

            // Повернення успішної відповіді
            return Ok(new { message = "Бібліотекар зареєстрований успішно" });
        }

        [HttpPost("register/reader")]
        public IActionResult RegisterReader([FromBody] RegisterReaderDto model)
        {
            // Виклик сервісу для реєстрації читача
            _authService.RegisterReader(model.Login, model.Password, model.Email, model.Forename, model.Surname, model.DocumentId, model.DocumentNumber);

            // Повернення успішної відповіді
            return Ok(new { message = "Читач зареєстрований успішно" });
        }
    }
}
