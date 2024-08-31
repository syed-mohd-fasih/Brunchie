using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brunchie.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddedContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentContact",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorContact",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentContact",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VendorContact",
                table: "AspNetUsers");
        }
    }
}
