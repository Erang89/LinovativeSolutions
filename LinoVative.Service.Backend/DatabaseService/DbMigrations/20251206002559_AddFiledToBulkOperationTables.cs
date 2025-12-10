using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddFiledToBulkOperationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Column2",
                schema: "temp",
                table: "ItemUnitBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum2",
                schema: "temp",
                table: "ItemUnitBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column2",
                schema: "temp",
                table: "ItemGroupBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum2",
                schema: "temp",
                table: "ItemGroupBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column3",
                schema: "temp",
                table: "ItemCategoryBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum3",
                schema: "temp",
                table: "ItemCategoryBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column7",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum7",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Column3",
                schema: "temp",
                table: "ItemCategoryBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "headerColum3",
                schema: "temp",
                table: "ItemCategoryBulkUpload");

            migrationBuilder.DropColumn(
                name: "Column7",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "headerColum7",
                schema: "temp",
                table: "ItemBulkUpload");
        }
    }
}
