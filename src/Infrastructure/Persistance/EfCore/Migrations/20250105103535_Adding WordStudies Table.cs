using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddingWordStudiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordStudies",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WordName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastStudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaysToStudy = table.Column<int>(type: "int", nullable: false),
                    StudyDaysLog = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordStudies", x => new { x.UserName, x.WordName });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordStudies");
        }
    }
}
