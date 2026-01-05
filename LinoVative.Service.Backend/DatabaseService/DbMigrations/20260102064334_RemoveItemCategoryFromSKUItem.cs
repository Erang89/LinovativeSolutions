using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RemoveItemCategoryFromSKUItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SKUItems_ItemCategories_CategoryId",
                table: "SKUItems");

            migrationBuilder.DropIndex(
                name: "IX_SKUItems_CategoryId",
                table: "SKUItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "SKUItems");

            migrationBuilder.AddColumn<string>(
                name: "VarianName",
                table: "SKUItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VarianName",
                table: "SKUItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "SKUItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SKUItems_CategoryId",
                table: "SKUItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SKUItems_ItemCategories_CategoryId",
                table: "SKUItems",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
