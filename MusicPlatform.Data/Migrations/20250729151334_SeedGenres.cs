using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "DeletedOn", "IsDeleted", "Name", "PublicId" },
                values: new object[,]
                {
                    { 1, null, false, "Electronic", new Guid("c0a9e70e-6e8a-4b0d-9b0d-0343a8b4e47a") },
                    { 2, null, false, "Hip-Hop", new Guid("d1b8e81f-7f9b-4e1a-9a9b-0454b9c5f58b") },
                    { 3, null, false, "Rock", new Guid("e2c7f92f-8a0c-4f2b-8b0c-0565cae6e69c") },
                    { 4, null, false, "Pop", new Guid("f3d6ea3f-9b1d-4e3c-7c1d-0676dbf7f7ad") },
                    { 5, null, false, "Classical", new Guid("04e5fb4f-a02e-4d4d-6d2e-0787ec08e8be") },
                    { 6, null, false, "Jazz", new Guid("15f40a5f-b13f-4f5e-5e3f-0898fd19f9cf") },
                    { 7, null, false, "Lo-Fi", new Guid("26031b6f-c24a-4e6f-4f4a-09a90e2a0ad0") },
                    { 8, null, false, "Ambient", new Guid("37122c7f-d35b-4d7f-3f5b-0ab91f3b1be1") },
                    { 9, null, false, "Other", new Guid("48213d8f-e46c-4c8f-2c6c-0bca204c2cf2") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
