using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class BrandAddedToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_BrandId",
                table: "Applications",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Brands_BrandId",
                table: "Applications",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Brands_BrandId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_BrandId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Applications");
        }
    }
}
