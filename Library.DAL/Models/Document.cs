using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public class Document
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
    }
}
