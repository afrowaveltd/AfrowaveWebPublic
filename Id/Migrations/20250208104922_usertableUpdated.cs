using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class usertableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailConfirmationTokenExpiration",
                table: "Users",
                newName: "OTPTokenExpiration");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmationToken",
                table: "Users",
                newName: "OTPToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OTPTokenExpiration",
                table: "Users",
                newName: "EmailConfirmationTokenExpiration");

            migrationBuilder.RenameColumn(
                name: "OTPToken",
                table: "Users",
                newName: "EmailConfirmationToken");
        }
    }
}
