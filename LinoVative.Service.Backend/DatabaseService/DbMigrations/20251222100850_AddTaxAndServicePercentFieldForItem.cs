using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddTaxAndServicePercentFieldForItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ServicePercent",
                table: "Items",
                type: "decimal(8,4)",
                precision: 8,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TaxAndServicePercentFromOutletOrderType",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercent",
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
                name: "ServicePercent",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TaxAndServicePercentFromOutletOrderType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TaxPercent",
                table: "Items");
        }
    }
}
