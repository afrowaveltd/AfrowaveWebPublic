using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class translation_log : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UiTranslatorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DefaultLanguageFound = table.Column<bool>(type: "bit", nullable: false),
                    TargetLanguagesCount = table.Column<int>(type: "int", nullable: false),
                    PhrazesCount = table.Column<int>(type: "int", nullable: false),
                    OldTranslationsFound = table.Column<bool>(type: "bit", nullable: false),
                    PhrazesToTranslateCount = table.Column<int>(type: "int", nullable: false),
                    PhrazesToRemoveCount = table.Column<int>(type: "int", nullable: false),
                    TranslatedPhrazesCount = table.Column<int>(type: "int", nullable: false),
                    TranslationErrorCount = table.Column<int>(type: "int", nullable: false),
                    TotalTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UiTranslatorLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UiTranslatorLogs");
        }
    }
}
