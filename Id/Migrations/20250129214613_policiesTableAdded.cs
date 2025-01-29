using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class policiesTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    OriginalLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationPolicies_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PolicyTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnapprovedContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyTranslations_ApplicationPolicies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "ApplicationPolicies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PolicyTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPolicies_ApplicationId",
                table: "ApplicationPolicies",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyTranslations_LanguageId",
                table: "PolicyTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyTranslations_PolicyId",
                table: "PolicyTranslations",
                column: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyTranslations");

            migrationBuilder.DropTable(
                name: "ApplicationPolicies");
        }
    }
}
