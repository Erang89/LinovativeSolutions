using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddPurchasingFlagsToItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DefaltPurchaseQty",
                table: "Items",
                type: "decimal(8,4)",
                precision: 8,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShouldPurchaseWhenStockLessOrEqualsTo",
                table: "Items",
                type: "decimal(8,4)",
                precision: 8,
                scale: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaltPurchaseQty",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ShouldPurchaseWhenStockLessOrEqualsTo",
                table: "Items");
        }
    }
}
