using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBook.Persist.Migrations
{
    public partial class PublicIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AspNetUsers");
        }
    }
}
