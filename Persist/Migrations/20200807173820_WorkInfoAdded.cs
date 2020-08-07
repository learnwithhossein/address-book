using Microsoft.EntityFrameworkCore.Migrations;

namespace Persist.Migrations
{
    public partial class WorkInfoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkAddress",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkNo",
                table: "Contacts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WorkAddress",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WorkNo",
                table: "Contacts");
        }
    }
}
