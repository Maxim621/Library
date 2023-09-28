using System;
using System.Collections.Generic;

namespace Library.DAL.Models
{

    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string PublisherCode { get; set; } = null!;

        public int? PublishingCodeId { get; set; }

        public int? Annum { get; set; }

        public string? PublishingCountry { get; set; }

        public string? CityOfPublishing { get; set; }

        public virtual BookCodeType? PublishingCode { get; set; }

        public List<Author> Authors { get; set; }

        public bool IsAvailable { get; set; }

        public int PublicationYear { get; set; }

    }
}
