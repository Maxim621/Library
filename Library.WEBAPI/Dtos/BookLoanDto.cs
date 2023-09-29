namespace Library.WEBAPI.Dtos
{
    public class BookLoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
