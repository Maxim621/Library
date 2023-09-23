using System;
using Library.DAL.Models;

namespace Library.DAL
{
    public class AuthService
    {
        private readonly LibrarianService _librarianService;
        private readonly ReaderService _readerService;

        public AuthService(LibrarianService librarianService, ReaderService readerService)
        {
            _librarianService = librarianService;
            _readerService = readerService;
        }

        public bool Login(string username, string password)
        {
            // Перевірка наявності користувача в базі даних (якщо користувачських даних немає в базі, видаємо помилку)

            if (_librarianService.Login(username, password))
            {
                // Якщо логін та пароль співпадають з бібліотекарем, повертаємо роль "Бібліотекар"
                return true;
            }
            else if (_readerService.Login(username, password))
            {
                // Якщо логін та пароль співпадають з читачем, повертаємо роль "Читач"
                return true;
            }

            // В інших випадках видаємо помилку автентифікації
            return false;
        }
    }
}
