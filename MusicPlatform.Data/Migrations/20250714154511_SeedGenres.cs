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
                    { 1, null, false, "Electronic", new Guid("5a85393c-5efd-4d35-9702-48d58d7bdac2") },
                    { 2, null, false, "Hip-Hop", new Guid("11a8fef7-657a-482b-b0d4-82f5f3dd2821") },
                    { 3, null, false, "Rock", new Guid("4677ee7d-3989-4eb4-b832-1d7cf84ac30d") },
                    { 4, null, false, "Pop", new Guid("300b6c62-1004-4694-ad68-bf22f7395c50") },
                    { 5, null, false, "Classical", new Guid("8ace07bb-4f1f-4c8d-8a4c-d342ed98f40e") },
                    { 6, null, false, "Jazz", new Guid("390fa0df-6258-4482-8efd-c9b9e37da7e7") },
                    { 7, null, false, "Lo-Fi", new Guid("a4f864c8-e7a0-45c1-b70c-afc78415c9db") },
                    { 8, null, false, "Ambient", new Guid("e26a9ccf-62b1-440c-b2ac-a725519cb963") },
                    { 9, null, false, "Other", new Guid("c1efad67-db52-426d-bbdb-b3c08557c88d") }
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
