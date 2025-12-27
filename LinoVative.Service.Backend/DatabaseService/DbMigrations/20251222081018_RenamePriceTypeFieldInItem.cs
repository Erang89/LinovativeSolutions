using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RenamePriceTypeFieldInItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_CostumePriceTagId",
                table: "ItemCustomePrices");

            migrationBuilder.RenameColumn(
                name: "CostumePriceTagId",
                table: "ItemCustomePrices",
                newName: "PriceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemCustomePrices_CostumePriceTagId",
                table: "ItemCustomePrices",
                newName: "IX_ItemCustomePrices_PriceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_PriceTypeId",
                table: "ItemCustomePrices",
                column: "PriceTypeId",
                principalTable: "PriceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_PriceTypeId",
                table: "ItemCustomePrices");

            migrationBuilder.RenameColumn(
                name: "PriceTypeId",
                table: "ItemCustomePrices",
                newName: "CostumePriceTagId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemCustomePrices_PriceTypeId",
                table: "ItemCustomePrices",
                newName: "IX_ItemCustomePrices_CostumePriceTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_CostumePriceTagId",
                table: "ItemCustomePrices",
                column: "CostumePriceTagId",
                principalTable: "PriceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
