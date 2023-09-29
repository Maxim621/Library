using System.ComponentModel.DataAnnotations;

namespace Library.WEBAPI.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }
    }
}
