using Microsoft.EntityFrameworkCore.Migrations;

namespace Emotify.Migrations
{
    public partial class filetypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "EmoteImages",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "EmoteImages");
        }
    }
}
