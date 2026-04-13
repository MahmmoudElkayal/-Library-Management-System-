using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Bio", "Name" },
                values: new object[,]
                {
                    { 5, "American novelist of the Jazz Age", "F. Scott Fitzgerald" },
                    { 6, "American novelist best known for To Kill a Mockingbird", "Harper Lee" },
                    { 7, "Lebanese-American writer, poet and visual artist", "Kahlil Gibran" },
                    { 8, "Afghan-American novelist and physician", "Khaled Hosseini" },
                    { 9, "Sudanese novelist and journalist", "الطيب صالح (Tayeb Salih)" },
                    { 10, "Palestinian writer and political activist", "غسان كنفاني (Ghassan Kanafani)" },
                    { 11, "Egyptian novelist, Nobel Prize laureate in Literature 1988", "نجيب محفوظ (Naguib Mahfouz)" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 5, "Classic and contemporary world literature", "Literature" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CategoryId", "CoverImage", "ISBN", "Title" },
                values: new object[,]
                {
                    { 11, 5, 2, "/images/books/great-gatsby.jpg", "978-0743273565", "The Great Gatsby" },
                    { 12, 6, 2, "/images/books/mockingbird.jpg", "978-0061120084", "To Kill a Mockingbird" },
                    { 13, 7, 5, "/images/books/prophet.jpg", "978-0394404288", "The Prophet" },
                    { 14, 8, 2, "/images/books/kite-runner.jpg", "978-1594631931", "The Kite Runner" },
                    { 15, 9, 2, "/images/books/season-migration.jpg", "978-0141190600", "موسم الهجرة إلى الشمال" },
                    { 16, 10, 2, "/images/books/men-in-sun.jpg", "978-0894107627", "رجال في الشمس" },
                    { 17, 11, 2, "/images/books/midaq-alley.jpg", "978-9774160615", "زقاق المدق" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
