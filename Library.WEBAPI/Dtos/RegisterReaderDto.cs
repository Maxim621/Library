namespace Library.WEBAPI.Dtos
{
    public class RegisterReaderDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int DocumentId { get; set; }
        public string DocumentNumber { get; set; }
    }
}
