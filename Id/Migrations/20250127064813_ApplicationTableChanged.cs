using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationCookiesPolicy",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationPrivacyPolicy",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationTermsAndConditions",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationCookiesPolicy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationPrivacyPolicy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationTermsAndConditions",
                table: "Applications");
        }
    }
}
