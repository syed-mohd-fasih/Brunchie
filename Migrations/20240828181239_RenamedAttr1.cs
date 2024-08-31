using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Brunchie.Migrations
{
    /// <inheritdoc />
    public partial class RenamedAttr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "OrderItem",
                newName: "ItemName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "OrderItem",
                newName: "MenuItemId");
        }
    }
}
