using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddSellingPriceAndTaxFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSellingTaxAndService",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSellingTaxAndService",
                table: "Items");
        }
    }
}
