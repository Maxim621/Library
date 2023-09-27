using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Library.DAL
{

    public class LibraryContext : DbContext
    {
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<BookCodeType> PublishingCodes { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Визначення зв'язку між читачем і документом (типом документа)
            modelBuilder.Entity<Reader>()
        .HasOne(r => r.Document)
        .WithMany()
        .HasForeignKey(r => r.DocumentId);

            // Визначення взаємозв'язку між позиченнями книг і користувачами (читачами)
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Reader)
                .WithMany(r => r.BookLoans)
                .HasForeignKey(bl => bl.ReaderId);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.Book)
                .WithMany()
                .HasForeignKey(bl => bl.BookId);

            // Додавання індексу для полів Email в користувачів
            modelBuilder.Entity<Librarian>()
                .HasIndex(l => l.Email)
                .IsUnique();

            modelBuilder.Entity<Reader>()
                .HasIndex(r => r.Email)
                .IsUnique();

            // Додавання індексу для полів Login в користувачів
            modelBuilder.Entity<Librarian>()
                .HasIndex(l => l.Login)
                .IsUnique();

            modelBuilder.Entity<Reader>()
                .HasIndex(r => r.Login)
                .IsUnique();
        }
    }
}
