using Library.DAL;
using Library.DAL.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LibraryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Завантаження конфігурації з файлу appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Налаштування DI контейнера
            var serviceProvider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddScoped<LibrarianService>()
                .AddScoped<ReaderService>()
                .AddScoped<LibraryContext>()
                .BuildServiceProvider();

            // Отримання сервісу та виклик методів
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                Console.WriteLine("Введіть команду і продовжуйте роботу з додатком.");
                string command = Console.ReadLine();

                if (command == "LoginAsLibrarian")
                {
                    Console.WriteLine("Введіть логін та пароль бібліотекаря:");
                    string login = Console.ReadLine();
                    string password = Console.ReadLine();

                    var librarianService = services.GetRequiredService<LibrarianService>();
                    bool loggedIn = librarianService.Login(login, password);

                    if (loggedIn)
                    {
                        Console.WriteLine("Ви увійшли в систему як бібліотекар.");

                        Console.WriteLine("Оберіть дію:");
                        Console.WriteLine("1. Переглянути список усіх книг");
                        Console.WriteLine("2. Переглянути список усіх авторів");
                        Console.WriteLine("3. Додати або оновити книгу");
                        Console.WriteLine("4. Додати або оновити автора");
                        Console.WriteLine("5. Видалити читача");
                        Console.WriteLine("6. Переглянути список боржників");
                        Console.WriteLine("7. Переглянути список усіх читачів");
                        Console.WriteLine("8. Переглянути історію взяття та повернення книг для конкретного читача");
                        Console.WriteLine("9. Вийти з системи");

                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                // Переглянути список усіх книг
                                var allBooks = librarianService.GetAllBooks();
                                foreach (var book in allBooks)
                                {
                                    Console.WriteLine($"Назва: {book.Title}, Автори: {string.Join(", ", book.Authors.Select(a => a.Surname))}");
                                }
                                break;
                            case "2":
                                // Переглянути список усіх авторів
                                var allAuthors = librarianService.GetAllAuthors();
                                foreach (var author in allAuthors)
                                {
                                    Console.WriteLine($"Автор: {author.Surname}");
                                }
                                break;
                            case "3":
                                Console.WriteLine("Введіть назву книги:");
                                string bookTitle = Console.ReadLine();

                                Console.WriteLine("Введіть рік видання:");
                                int publicationYear = int.Parse(Console.ReadLine());

                                // Введіть інші необхідні атрибути книги, які потрібні для ініціалізації об'єкта Book

                                // Створення об'єкта книги
                                var newBook = new Book
                                {
                                    Title = bookTitle,
                                    PublicationYear = publicationYear,
                                    // Ініціалізуйте інші атрибути книги тут
                                };

                                // Додавання або оновлення книги в базу даних
                                librarianService.AddOrUpdateBook(newBook);

                                Console.WriteLine("Книгу було додано або оновлено.");
                                break;
                            case "4":
                                Console.WriteLine("Введіть ім'я автора:");
                                string authorForename = Console.ReadLine();

                                Console.WriteLine("Введіть прізвище автора:");
                                string authorSurname = Console.ReadLine();

                                // Введіть інші необхідні атрибути автора, які потрібні для ініціалізації об'єкта Author

                                // Створення об'єкта автора
                                var newAuthor = new Author
                                {
                                    Forename = authorForename,
                                    Surname = authorSurname,
                                    // Ініціалізуйте інші атрибути автора тут
                                };

                                // Додавання або оновлення автора в базу даних
                                librarianService.AddOrUpdateAuthor(newAuthor);

                                Console.WriteLine("Автора було додано або оновлено.");
                                break;
                            case "5":
                                // Видалити читача
                                Console.WriteLine("Введіть ID читача, якого потрібно видалити:");
                                int readerIdToDelete = int.Parse(Console.ReadLine());
                                librarianService.DeleteReader(readerIdToDelete);
                                Console.WriteLine("Читача було видалено.");
                                break;
                            case "6":
                                // Переглянути список боржників
                                var debtors = librarianService.GetDebtors();
                                foreach (var debtor in debtors)
                                {
                                    Console.WriteLine($"Читач: {debtor.Surname}");
                                }
                                break;
                            case "7":
                                // Переглянути список усіх читачів
                                var allReaders = librarianService.GetAllReaders();
                                foreach (var reader in allReaders)
                                {
                                    Console.WriteLine($"Читач: {reader.Surname}");
                                }
                                break;
                            case "8":
                                // Переглянути історію взяття та повернення книг для конкретного читача
                                Console.WriteLine("Введіть ID читача для перегляду історії:");
                                int readerIdToViewHistory = int.Parse(Console.ReadLine());
                                var borrowHistory = librarianService.GetBorrowHistory(readerIdToViewHistory);
                                foreach (var historyItem in borrowHistory)
                                {
                                    Console.WriteLine($"Книга: {historyItem.Book.Title}, Дата взяття: {historyItem.DateBorrowed}, Дата повернення: {historyItem.DateReturned}");
                                }
                                break;
                            case "9":
                                // Вийти з системи
                                Console.WriteLine("Ви вийшли з системи.");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Невідома команда.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Помилка входу. Перевірте логін та пароль.");
                    }
                }
                else if (command == "LoginAsReader")
                {
                    Console.WriteLine("Введіть логін та пароль читача:");
                    string login = Console.ReadLine();
                    string password = Console.ReadLine();

                    var readerService = services.GetRequiredService<ReaderService>();
                    bool loggedIn = readerService.Login(login, password);

                    int readerId = 0;

                    if (loggedIn)
                    {
                        Console.WriteLine("Ви увійшли в систему як читач.");
                        readerId = readerService.GetReaderIdByLogin(login);

                        // Меню для читача
                        bool isRunning = true;
                        while (isRunning)
                        {
                            Console.WriteLine("Оберіть дію:");
                            Console.WriteLine("1. Пошук книг за автором");
                            Console.WriteLine("2. Пошук книг за назвою");
                            Console.WriteLine("3. Переглянути взяті книги");
                            Console.WriteLine("4. Взяти книгу");
                            Console.WriteLine("5. Вихід");
                            string userChoice = Console.ReadLine();

                            switch (userChoice)
                            {
                                case "1":
                                    // Логіка для пошуку книг за автором
                                    Console.WriteLine("Введіть ім'я або прізвище автора:");
                                    string authorName = Console.ReadLine();

                                    var booksByAuthor = readerService.SearchBooksByAuthor(authorName);

                                    if (booksByAuthor.Count > 0)
                                    {
                                        Console.WriteLine("Результати пошуку:");
                                        foreach (var book in booksByAuthor)
                                        {
                                            Console.WriteLine($"Назва: {book.Title}, Автор(и): {string.Join(", ", book.Authors.Select(a => a.Forename + " " + a.Surname))}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Книги автора не знайдено.");
                                    }
                                    break;

                                case "2":
                                    // Логіка для пошуку книг за назвою
                                    Console.WriteLine("Введіть назву книги:");
                                    string bookTitle = Console.ReadLine();

                                    var booksByTitle = readerService.SearchBooksByTitle(bookTitle);

                                    if (booksByTitle.Count > 0)
                                    {
                                        Console.WriteLine("Результати пошуку:");
                                        foreach (var book in booksByTitle)
                                        {
                                            Console.WriteLine($"Назва: {book.Title}, Автор(и): {string.Join(", ", book.Authors.Select(a => a.Forename + " " + a.Surname))}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Книга не знайдена.");
                                    }
                                    break;

                                case "3":
                                    // Логіка для перегляду взятих книг
                                    var borrowedBooks = readerService.GetBorrowedBooks(readerId);

                                    if (borrowedBooks.Count > 0)
                                    {
                                        Console.WriteLine("Ваші взяті книги:");
                                        foreach (var loan in borrowedBooks)
                                        {
                                            var book = loan.Book;
                                            Console.WriteLine($"Назва: {book.Title}, Дата взяття: {loan.DateBorrowed}, Дата повернення: {loan.DateReturned}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ви не взяли жодної книги.");
                                    }
                                    break;

                                case "4":
                                    // Логіка для взяття книги
                                    Console.WriteLine("Введіть ID книги, яку ви хочете взяти:");
                                    if (int.TryParse(Console.ReadLine(), out int bookId))
                                    {
                                        readerService.BorrowBook(bookId, readerId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Некоректний ID книги.");
                                    }
                                    break;

                                case "5":
                                    isRunning = false;
                                    Console.WriteLine("До побачення!");
                                    break;

                                default:
                                    Console.WriteLine("Невідома команда.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Помилка входу. Перевірте логін та пароль.");
                    }
                }
            }
        }
    }
}
