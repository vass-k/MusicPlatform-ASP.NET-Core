using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ef6eeff1-af06-48e3-8c83-703cf53d5ba9", 0, "693da562-ddf1-4c65-8149-1cfb8222748c", "default@music.com", true, false, null, "DEFAULT@MUSIC.COM", "DEFAULT@MUSIC.COM", "AQAAAAIAAYagAAAAEPbvz3hUaf/q7G77Gmyk3Ha5nIzbd23lrVLVgO4iClRwqJt/GJtQducTsWrddTom3Q==", null, false, "QTQXHOC5V5FS6EVBCHVWL2EMYYO3MSRA", false, "default@music.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ef6eeff1-af06-48e3-8c83-703cf53d5ba9");
        }
    }
}
