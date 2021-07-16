using Microsoft.EntityFrameworkCore.Migrations;

namespace Emotify.Migrations
{
    public partial class imagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emotes_EmoteImage_EmoteImageId",
                table: "Emotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmoteImage",
                table: "EmoteImage");

            migrationBuilder.RenameTable(
                name: "EmoteImage",
                newName: "EmoteImages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmoteImages",
                table: "EmoteImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Emotes_EmoteImages_EmoteImageId",
                table: "Emotes",
                column: "EmoteImageId",
                principalTable: "EmoteImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emotes_EmoteImages_EmoteImageId",
                table: "Emotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmoteImages",
                table: "EmoteImages");

            migrationBuilder.RenameTable(
                name: "EmoteImages",
                newName: "EmoteImage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmoteImage",
                table: "EmoteImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Emotes_EmoteImage_EmoteImageId",
                table: "Emotes",
                column: "EmoteImageId",
                principalTable: "EmoteImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
