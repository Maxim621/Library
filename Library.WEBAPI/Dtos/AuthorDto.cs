using System.ComponentModel.DataAnnotations;

namespace Library.WEBAPI.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}
