using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class FixingBulkUploadDatabaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column2",
                schema: "temp",
                table: "ItemUnitBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "headerColum2",
                schema: "temp",
                table: "ItemUnitBulkUpload");

            migrationBuilder.DropColumn(
                name: "Column2",
                schema: "temp",
                table: "ItemGroupBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "headerColum2",
                schema: "temp",
                table: "ItemGroupBulkUpload");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Column2",
                schema: "temp",
                table: "ItemUnitBulkUploadDetail",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum2",
                schema: "temp",
                table: "ItemUnitBulkUpload",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column2",
                schema: "temp",
                table: "ItemGroupBulkUploadDetail",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum2",
                schema: "temp",
                table: "ItemGroupBulkUpload",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
