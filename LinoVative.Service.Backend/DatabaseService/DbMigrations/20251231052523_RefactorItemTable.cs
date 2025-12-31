using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RefactorItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_UnitId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_UnitId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HasCostumePrice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HasSellingTaxAndService",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SellPriceIncludeTaxService",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "TaxPercent",
                table: "Items",
                newName: "DefaultSellTaxPercent");

            migrationBuilder.RenameColumn(
                name: "TaxAndServicePercentFromOutletOrderType",
                table: "Items",
                newName: "CanBeSell");

            migrationBuilder.RenameColumn(
                name: "ShouldPurchaseWhenStockLessOrEqualsTo",
                table: "Items",
                newName: "DefaultSellServicePercent");

            migrationBuilder.RenameColumn(
                name: "ServicePercent",
                table: "Items",
                newName: "DefaultMinimumStock");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Items",
                newName: "Notes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemCustomePrices",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Items",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DefaultSellTaxPercent",
                table: "Items",
                newName: "TaxPercent");

            migrationBuilder.RenameColumn(
                name: "DefaultSellServicePercent",
                table: "Items",
                newName: "ShouldPurchaseWhenStockLessOrEqualsTo");

            migrationBuilder.RenameColumn(
                name: "DefaultMinimumStock",
                table: "Items",
                newName: "ServicePercent");

            migrationBuilder.RenameColumn(
                name: "CanBeSell",
                table: "Items",
                newName: "TaxAndServicePercentFromOutletOrderType");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCostumePrice",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSellingTaxAndService",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SellPrice",
                table: "Items",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "SellPriceIncludeTaxService",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "ItemCustomePrices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UnitId",
                table: "Items",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_UnitId",
                table: "Items",
                column: "UnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id");
        }
    }
}
