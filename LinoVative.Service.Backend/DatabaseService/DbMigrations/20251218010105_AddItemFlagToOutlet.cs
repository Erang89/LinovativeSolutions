using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddItemFlagToOutlet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseAllItemGroup",
                table: "Outlets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseAllItemCategories",
                table: "OutletItemGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseAllItemGroup",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "UseAllItemCategories",
                table: "OutletItemGroups");
        }
    }
}
