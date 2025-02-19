using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistance.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pronunciation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PronunciationAm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeaningShort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeaningLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Example = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExampleSoundUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SimilarWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "WordStudies",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WordName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastStudyTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudyCount = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    DaysToStudy = table.Column<int>(type: "int", nullable: false),
                    DaysToStudyHistory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordStudies", x => new { x.UserName, x.WordName });
                    table.ForeignKey(
                        name: "FK_WordStudies_Words_WordName",
                        column: x => x.WordName,
                        principalTable: "Words",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordStudies_WordName",
                table: "WordStudies",
                column: "WordName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordStudies");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
