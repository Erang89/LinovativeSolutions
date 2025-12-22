using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RenamePriceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_ItemPriceTags_CostumePriceTagId",
                table: "ItemCustomePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemPriceTags",
                table: "ItemPriceTags");

            migrationBuilder.RenameTable(
                name: "ItemPriceTags",
                newName: "PriceTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceTypes",
                table: "PriceTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_CostumePriceTagId",
                table: "ItemCustomePrices",
                column: "CostumePriceTagId",
                principalTable: "PriceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCustomePrices_PriceTypes_CostumePriceTagId",
                table: "ItemCustomePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceTypes",
                table: "PriceTypes");

            migrationBuilder.RenameTable(
                name: "PriceTypes",
                newName: "ItemPriceTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemPriceTags",
                table: "ItemPriceTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCustomePrices_ItemPriceTags_CostumePriceTagId",
                table: "ItemCustomePrices",
                column: "CostumePriceTagId",
                principalTable: "ItemPriceTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
