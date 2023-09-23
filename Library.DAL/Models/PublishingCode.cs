using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public partial class PublishingCode
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
