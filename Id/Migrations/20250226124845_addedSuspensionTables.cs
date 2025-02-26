using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class addedSuspensionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_SuspendedById",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_SuspendedById",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SuspendedById",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "SuspendedApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuspenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuspensionActive = table.Column<bool>(type: "bit", nullable: false),
                    SuspendedFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuspendedUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspendedApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspendedApplications_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuspendedApplications_Users_SuspenderId",
                        column: x => x.SuspenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SuspendedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    SuspenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SuspensionActive = table.Column<bool>(type: "bit", nullable: false),
                    SuspendedFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuspendedUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspendedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspendedUsers_ApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuspendedUsers_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SuspendedUsers_Users_SuspenderId",
                        column: x => x.SuspenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedApplications_ApplicationId",
                table: "SuspendedApplications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedApplications_SuspenderId",
                table: "SuspendedApplications",
                column: "SuspenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedUsers_ApplicationId",
                table: "SuspendedUsers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedUsers_ApplicationUserId",
                table: "SuspendedUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SuspendedUsers_SuspenderId",
                table: "SuspendedUsers",
                column: "SuspenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuspendedApplications");

            migrationBuilder.DropTable(
                name: "SuspendedUsers");

            migrationBuilder.AddColumn<string>(
                name: "SuspendedById",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_SuspendedById",
                table: "Applications",
                column: "SuspendedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_SuspendedById",
                table: "Applications",
                column: "SuspendedById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
