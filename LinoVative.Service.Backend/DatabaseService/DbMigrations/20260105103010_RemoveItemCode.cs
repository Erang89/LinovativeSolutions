using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RemoveItemCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
