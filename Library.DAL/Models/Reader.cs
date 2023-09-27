using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public class Reader : Librarian
    {
        public string Forename { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int? DocumentId { get; set; }

        public string? DocumentNumber { get; set; }

        public virtual Document? Document { get; set; }

        public List<BookLoan> BookLoans { get; set; } // Відношення багато-до-багатьох до позик книг
        
        public bool IsDebtor { get; set; }
    }
}
