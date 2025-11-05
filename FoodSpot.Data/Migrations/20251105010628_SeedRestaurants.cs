using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodSpot.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRestaurants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comida Italiana", "Italiana" },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comida Japonesa", "Japonesa" },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comida Brasileira", "Brasileira" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "AverageRating", "CategoryId", "CreatedAt", "Description", "ImageUrl", "Name", "Phone", "ReviewCount" },
                values: new object[,]
                {
                    { 1, "Rua das Flores, 123 - São Paulo/SP", 4.5, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Autêntica comida italiana com receitas tradicionais da Toscana", "https://via.placeholder.com/400x200?text=La+Pasta", "La Pasta", "(11) 3000-0000", 10 },
                    { 2, "Av. Paulista, 456 - São Paulo/SP", 4.7999999999999998, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "O melhor sushi e sashimi da cidade, com peixes frescos diariamente", "https://via.placeholder.com/400x200?text=Sakura", "Sakura Sushi", "(11) 3001-0000", 25 },
                    { 3, "Rua dos Pampas, 789 - São Paulo/SP", 4.7000000000000002, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Carne de primeira qualidade no sistema rodízio", "https://via.placeholder.com/400x200?text=Churrascaria", "Churrascaria Gaúcha", "(11) 3002-0000", 15 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
