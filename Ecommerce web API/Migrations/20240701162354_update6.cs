using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_web_API.Migrations
{
    /// <inheritdoc />
    public partial class update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Products",
                newName: "SellerId");

            migrationBuilder.AddColumn<int>(
                name: "ammount",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ammount",
                table: "ShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Products",
                newName: "UserId");
        }
    }
}
