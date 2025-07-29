using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "ArtistName", "AudioUrl", "CreatedOn", "DeletedOn", "DurationInSeconds", "GenreId", "ImageUrl", "IsDeleted", "Plays", "PublicId", "Title", "UploaderId" },
                values: new object[,]
                {
                    { 1, "BoDleasons", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805872/zaharibaharov-track_z6wshx.mp3", new DateTime(2025, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), null, 122, 7, null, false, 0, new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"), "Lofi Chill", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 2, "AlexiAction", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805884/garniertaip-track_jovkcj.mp3", new DateTime(2025, 2, 20, 14, 0, 0, 0, DateTimeKind.Utc), null, 183, 1, null, false, 0, new Guid("b2c3d4e5-f6a7-8901-2345-67890abcdef1"), "Midnight Drive", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 3, "AlexGrohl", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805887/happy-track_kdyndk.mp3", new DateTime(2025, 3, 1, 18, 45, 0, 0, DateTimeKind.Utc), null, 130, 3, null, false, 0, new Guid("c3d4e5f6-a7b8-9012-3456-7890abcdef12"), "The Power of Rock", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 4, "Sun Chasers", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805890/some-track_wbnomn.mp3", new DateTime(2025, 3, 5, 11, 0, 0, 0, DateTimeKind.Utc), null, 145, 4, null, false, 0, new Guid("d4e5f6a7-b8c9-0123-4567-890abcdef123"), "Summer Glow", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 5, "Celestial Sound", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805892/RnB-track_xba7dj.mp3", new DateTime(2025, 3, 10, 22, 15, 0, 0, DateTimeKind.Utc), null, 195, 8, null, false, 0, new Guid("e5f6a7b8-c9d0-1234-5678-90abcdef1234"), "Stardust Echoes", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 6, "Beat Architect", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805893/kitara-track_ekkqbm.mp3", new DateTime(2025, 3, 12, 9, 5, 0, 0, DateTimeKind.Utc), null, 110, 2, null, false, 0, new Guid("f6a7b8c9-d0e1-2345-6789-0abcdef12345"), "Urban Canvas", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 7, "The Midnight Trio", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805896/house-track_tdretg.mp3", new DateTime(2025, 3, 18, 23, 0, 0, 0, DateTimeKind.Utc), null, 155, 6, null, false, 0, new Guid("a7b8c9d0-e1f2-3456-7890-bcdef1234567"), "Smoky Lounge", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 8, "Serene Strings", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805898/hiphop-track_uaboy6.mp3", new DateTime(2025, 3, 25, 7, 30, 0, 0, DateTimeKind.Utc), null, 210, 5, null, false, 0, new Guid("b8c9d0e1-f2a3-4567-8901-cdef12345678"), "Default Image Track", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 9, "Cozy Beats", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805902/lofi-track_ydkbqj.mp3", new DateTime(2025, 4, 2, 16, 20, 0, 0, DateTimeKind.Utc), null, 140, 7, null, false, 0, new Guid("c9d0e1f2-a3b4-5678-9012-def123456789"), "Rainy Afternoon", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 10, "Studio 54", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805908/midnight-track_kcrawr.mp3", new DateTime(2025, 4, 8, 19, 0, 0, 0, DateTimeKind.Utc), null, 172, 1, null, false, 0, new Guid("d0e1f2a3-b4c5-6789-0123-ef1234567890"), "Deep Down", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 11, "Neon Bloom", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805944/50-track_xfmmbx.wav", new DateTime(2025, 4, 15, 12, 10, 0, 0, DateTimeKind.Utc), null, 160, 4, null, false, 0, new Guid("e1f2a3b4-c5d6-7890-1234-f12345678901"), "Electric Dreams", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" },
                    { 12, "Stone Temple", "https://res.cloudinary.com/dx9py9gkr/video/upload/v1753805989/wheezy-track_gdccms.wav", new DateTime(2025, 4, 22, 17, 55, 0, 0, DateTimeKind.Utc), null, 205, 3, null, false, 0, new Guid("f2a3b4c5-d6e7-8901-2345-123456789012"), "Mountain King", "ef6eeff1-af06-48e3-8c83-703cf53d5ba9" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
