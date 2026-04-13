using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooksData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CoverImage", "ISBN", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "/images/books/brief-history.jpg", "978-0553380163", "A Brief History of Time" },
                    { 2, 1, 1, "/images/books/universe-nutshell.jpg", "978-0553802023", "The Universe in a Nutshell" },
                    { 3, 2, 2, "/images/books/hp-stone.jpg", "978-0590353427", "Harry Potter and the Sorcerer's Stone" },
                    { 4, 2, 2, "/images/books/hp-chamber.jpg", "978-0439064873", "Harry Potter and the Chamber of Secrets" },
                    { 5, 2, 2, "/images/books/hp-azkaban.jpg", "978-0439136365", "Harry Potter and the Prisoner of Azkaban" },
                    { 6, 3, 3, "/images/books/sapiens.jpg", "978-0062316097", "Sapiens: A Brief History of Humankind" },
                    { 7, 3, 3, "/images/books/homo-deus.jpg", "978-0062464316", "Homo Deus: A Brief History of Tomorrow" },
                    { 8, 3, 3, "/images/books/21-lessons.jpg", "978-0525512172", "21 Lessons for the 21st Century" },
                    { 9, 4, 4, "/images/books/pro-csharp.jpg", "978-1484273616", "Pro C# 10 with .NET 6" },
                    { 10, 4, 4, "/images/books/dotnet-platform.jpg", "978-1590590558", "C# and the .NET Platform" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
