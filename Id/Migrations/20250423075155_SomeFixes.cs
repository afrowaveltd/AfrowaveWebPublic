using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Id.Migrations
{
    /// <inheritdoc />
    public partial class SomeFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThemeDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    FontLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foreground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormForeground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkHover = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkActive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedBorder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisabledBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisabledForeground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormControlValidBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormControlInvalidBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Navbar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Success = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Warning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Highlight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    My = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BorderLight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shadow = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModalBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Font = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemeDefinition_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThemeDefinition_ThemeId",
                table: "ThemeDefinition",
                column: "ThemeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeDefinition");
        }
    }
}
