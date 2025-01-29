using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequireConsent",
                table: "Applications",
                newName: "RequireTerms");

            migrationBuilder.AddColumn<bool>(
                name: "RequireCookiePolicy",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePrivacyPolicy",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireCookiePolicy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RequirePrivacyPolicy",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "RequireTerms",
                table: "Applications",
                newName: "RequireConsent");
        }
    }
}
