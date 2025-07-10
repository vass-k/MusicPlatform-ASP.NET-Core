using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentFeatureAndTrackDatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tracks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "The date and time when the track was uploaded to the platform.");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "The internal integer id that's the primary key for the comment.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "The text content of the comment."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the comment was posted."),
                    TrackId = table.Column<int>(type: "int", nullable: false, comment: "The foreign key of the track this comment belongs to."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "The foreign key of the user who posted this comment."),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Flag that shows if the comment is soft deleted."),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Timestamp when the comment was soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TrackId",
                table: "Comments",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tracks");
        }
    }
}
