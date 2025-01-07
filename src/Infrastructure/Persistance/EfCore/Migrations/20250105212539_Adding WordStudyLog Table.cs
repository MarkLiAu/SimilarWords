using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddingWordStudyLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudyDaysLog",
                table: "WordStudies");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WordStudies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "WordStudies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WordStudyLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordStudyId = table.Column<int>(type: "int", nullable: false),
                    StudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledStudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WordStudyUserName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    WordStudyWordName = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordStudyLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordStudyLog_WordStudies_WordStudyUserName_WordStudyWordName",
                        columns: x => new { x.WordStudyUserName, x.WordStudyWordName },
                        principalTable: "WordStudies",
                        principalColumns: new[] { "UserName", "WordName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordStudyLog_WordStudyUserName_WordStudyWordName",
                table: "WordStudyLog",
                columns: new[] { "WordStudyUserName", "WordStudyWordName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordStudyLog");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WordStudies");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "WordStudies");

            migrationBuilder.AddColumn<string>(
                name: "StudyDaysLog",
                table: "WordStudies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
