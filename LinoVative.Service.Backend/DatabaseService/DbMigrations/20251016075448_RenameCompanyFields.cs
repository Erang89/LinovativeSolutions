using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RenameCompanyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ItemUnits",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Items",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ItemPriceTags",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ItemGroups",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ItemCategories",
                newName: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ItemUnits",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Items",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ItemPriceTags",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ItemGroups",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ItemCategories",
                newName: "ClientId");
        }
    }
}
