using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class CreateSKUItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices");

            migrationBuilder.DropIndex(
                name: "IX_ItemCustomePrices_ItemId",
                table: "ItemCustomePrices");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemCustomePrices");

            migrationBuilder.AddColumn<Guid>(
                name: "SKUItemId",
                table: "ItemCustomePrices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SKUItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HasSaleTaxAndService = table.Column<bool>(type: "bit", nullable: false),
                    SalePriceIncludeTaxAndService = table.Column<bool>(type: "bit", nullable: false),
                    UseOutletSaleTaxAndService = table.Column<bool>(type: "bit", nullable: false),
                    DefaultPurchaseQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MinimumStockQty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    HasCostumePrice = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SKUItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SKUItems_ItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SKUItems_ItemUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ItemUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SKUItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCustomePrices_SKUItemId",
                table: "ItemCustomePrices",
                column: "SKUItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SKUItems_CategoryId",
                table: "SKUItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SKUItems_ItemId",
                table: "SKUItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SKUItems_UnitId",
                table: "SKUItems",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_SKUItems_SKUItemId",
                table: "ItemCustomePrices",
                column: "SKUItemId",
                principalTable: "SKUItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_SKUItems_SKUItemId",
                table: "ItemCustomePrices");

            migrationBuilder.DropTable(
                name: "SKUItems");

            migrationBuilder.DropIndex(
                name: "IX_ItemCustomePrices_SKUItemId",
                table: "ItemCustomePrices");

            migrationBuilder.DropColumn(
                name: "SKUItemId",
                table: "ItemCustomePrices");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "ItemCustomePrices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCustomePrices_ItemId",
                table: "ItemCustomePrices",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_Items_ItemId",
                table: "ItemCustomePrices",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
