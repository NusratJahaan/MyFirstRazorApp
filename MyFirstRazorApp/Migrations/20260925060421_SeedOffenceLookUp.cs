using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyFirstRazorApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedOffenceLookUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OffenceLookUps",
                columns: new[] { "Id", "Code", "Description" },
                values: new object[,]
                {
                    { 1, "14-40.1", "Domestic Violence" },
                    { 2, "20-141", "Speed Offence" },
                    { 3, "90-95", "Traffic Violation" },
                    { 4, "14-33", "Assault" },
                    { 5, "14-72", "Theft" },
                    { 6, "14-100", "Fraud" },
                    { 7, "14-196", "Trespassing" },
                    { 8, "14-277", "Disorderly Conduct" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OffenceLookUps",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
