using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brunchie.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AddedVendorContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "University",
                table: "AspNetUsers",
                newName: "CampusName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CampusName",
                table: "AspNetUsers",
                newName: "University");
        }
    }
}
