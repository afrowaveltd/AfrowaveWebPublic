using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class AddedMarkForAuthenticatorApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthenticator",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAuthenticator",
                table: "Applications");
        }
    }
}
