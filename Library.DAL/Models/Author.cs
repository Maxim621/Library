using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public partial class Author
    {
        public int Id { get; set; }

        public string Forename { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? SecondName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public List<Book> Books { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
