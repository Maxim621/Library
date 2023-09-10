using System;
using System.Collections.Generic;
using Library.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Librarian> Librarians { get; set; }

    public virtual DbSet<PublishingCode> PublishingCodes { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-M5BKIQ7\\MSSQLSERVER01;Database=Library;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3214EC27372F52AD");

            entity.ToTable("Author");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Forename).HasMaxLength(255);
            entity.Property(e => e.SecondName).HasMaxLength(255);
            entity.Property(e => e.Surname).HasMaxLength(255);

            entity.HasMany(d => d.Books).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorsBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AuthorsBo__Book___45F365D3"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AuthorsBo__Autho__44FF419A"),
                    j =>
                    {
                        j.HasKey("AuthorId", "BookId").HasName("PK__AuthorsB__099BC9E6BB727AB9");
                        j.ToTable("AuthorsBooks");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("Author_ID");
                        j.IndexerProperty<int>("BookId").HasColumnName("Book_ID");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC273D98925B");

            entity.ToTable("Book");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CityOfPublishing)
                .HasMaxLength(255)
                .HasColumnName("City_of_publishing");
            entity.Property(e => e.PublisherCode).HasMaxLength(255);
            entity.Property(e => e.PublishingCodeId).HasColumnName("PublishingCode_ID");
            entity.Property(e => e.PublishingCountry)
                .HasMaxLength(255)
                .HasColumnName("Publishing_country");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.PublishingCode).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublishingCodeId)
                .HasConstraintName("FK__Book__Publishing__3D5E1FD2");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC27368151B9");

            entity.ToTable("Document");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Librarian>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Libraria__3214EC27CACA2498");

            entity.ToTable("Librarian");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<PublishingCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishi__3214EC2740A4C538");

            entity.ToTable("PublishingCode");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reader__3214EC27543D4597");

            entity.ToTable("Reader");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DocumentId).HasColumnName("Document_ID");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(255)
                .HasColumnName("Document_number");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Forename).HasMaxLength(255);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Surname).HasMaxLength(255);

            entity.HasOne(d => d.Document).WithMany(p => p.Readers)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK__Reader__Document__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public class LibrarianService
{
    private readonly LibraryContext _context;

    public LibrarianService()
    {
        _context = new LibraryContext();
    }

    public bool LoginToTheSystem(string login, string password)
    {
        // Перевірка наявності бібліотекаря з введеним логіном і паролем у базі даних
        var librarian = _context.Librarians
            .FirstOrDefault(b => b.Login == login && b.Password == password);

        return librarian != null;
    }

    public void LibrarianRegistration(Librarian newLibrarian)
    {
        // Додавання нового бібліотекаря до бази даних
        _context.Librarians.Add(newLibrarian);
        _context.SaveChanges();
    }
}
