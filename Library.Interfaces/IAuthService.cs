using System;
using System.Collections.Generic;

namespace Library.Interfaces
{
    public interface IAuthService
    {
        bool Login(string login, string password);
        public void RegisterLibrarian(string login, string password, string email);
        public void RegisterReader(string login, string password, string email, string forename, string surname, int documentId, string documentNumber);
    }
}
