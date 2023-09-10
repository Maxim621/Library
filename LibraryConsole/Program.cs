using Library.DAL;
using Library.DAL.Models;
using System;

namespace LibraryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ласкаво просимо до бібліотеки!");

            // Основний цикл програми
            while (true)
            {
                Console.WriteLine("Оберіть опцію:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Sign up");
                Console.WriteLine("3. Вийти з програми");
                Console.Write("Ваш вибір: ");

                string вибір = Console.ReadLine();

                switch (вибір)
                {
                    case "1":
                        Login(); // Виклик функції для входу в систему
                        break;
                    case "2":
                        SignUp(); // Виклик функції для реєстрації
                        break;
                    case "3":
                        Console.WriteLine("Дякую за використання програми. До побачення!");
                        Environment.Exit(0); // Завершення програми
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void Login()
        {
            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            var librarianService = new LibrarianService();

            if (librarianService.LoginToTheSystem(login, password))
            {
                Console.WriteLine("Ви успішно увійшли в систему.");
            }
            else
            {
                Console.WriteLine("Неправильний логін або пароль. Спробуйте ще раз.");
            }
        }

        static void SignUp()
        {
            Console.Write("Введіть логін: ");
            string login = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            var librarianService = new LibrarianService();

            var newLibrarian = new Librarian
            {
                Login = login,
                Password = password,
            };

            librarianService.LibrarianRegistration(newLibrarian);

            Console.WriteLine("Ви успішно зареєструвалися. Тепер можете увійти в систему.");
        }
    }
}
