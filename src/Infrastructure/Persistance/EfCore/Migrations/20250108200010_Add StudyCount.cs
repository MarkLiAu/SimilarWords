using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordStudyLog");

            migrationBuilder.CreateIndex(
                name: "IX_WordStudies_WordName",
                table: "WordStudies",
                column: "WordName");

            migrationBuilder.AddForeignKey(
                name: "FK_WordStudies_Words_WordName",
                table: "WordStudies",
                column: "WordName",
                principalTable: "Words",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordStudies_Words_WordName",
                table: "WordStudies");

            migrationBuilder.DropIndex(
                name: "IX_WordStudies_WordName",
                table: "WordStudies");

            migrationBuilder.CreateTable(
                name: "WordStudyLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduledStudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WordStudyId = table.Column<int>(type: "int", nullable: false),
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
    }
}
