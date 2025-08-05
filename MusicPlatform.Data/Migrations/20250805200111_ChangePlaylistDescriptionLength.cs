using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlaylistDescriptionLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "An optional description for the playlist.",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "An optional description for the playlist.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Playlists",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "An optional description for the playlist.",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "An optional description for the playlist.");
        }
    }
}
