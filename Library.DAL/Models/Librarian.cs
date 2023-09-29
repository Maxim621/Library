using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public class Librarian
    {
        public int Id { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool Authenticate(string password)
        {
            return Password == password;
        }
    }
}
