using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Forename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Author__3214EC27372F52AD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__3214EC27368151B9", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Librarian",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Libraria__3214EC27CACA2498", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PublishingCode",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Publishi__3214EC2740A4C538", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reader",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Forename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Document_ID = table.Column<int>(type: "int", nullable: true),
                    Document_number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reader__3214EC27543D4597", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Reader__Document__4222D4EF",
                        column: x => x.Document_ID,
                        principalTable: "Document",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PublisherCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PublishingCode_ID = table.Column<int>(type: "int", nullable: true),
                    Annum = table.Column<int>(type: "int", nullable: true),
                    Publishing_country = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City_of_publishing = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Book__3214EC273D98925B", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Book__Publishing__3D5E1FD2",
                        column: x => x.PublishingCode_ID,
                        principalTable: "PublishingCode",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AuthorsBooks",
                columns: table => new
                {
                    Author_ID = table.Column<int>(type: "int", nullable: false),
                    Book_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuthorsB__099BC9E6BB727AB9", x => new { x.Author_ID, x.Book_ID });
                    table.ForeignKey(
                        name: "FK__AuthorsBo__Autho__44FF419A",
                        column: x => x.Author_ID,
                        principalTable: "Author",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__AuthorsBo__Book___45F365D3",
                        column: x => x.Book_ID,
                        principalTable: "Book",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsBooks_Book_ID",
                table: "AuthorsBooks",
                column: "Book_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Book_PublishingCode_ID",
                table: "Book",
                column: "PublishingCode_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reader_Document_ID",
                table: "Reader",
                column: "Document_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorsBooks");

            migrationBuilder.DropTable(
                name: "Librarian");

            migrationBuilder.DropTable(
                name: "Reader");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "PublishingCode");
        }
    }
}
