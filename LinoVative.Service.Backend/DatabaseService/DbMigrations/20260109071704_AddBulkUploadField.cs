using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddBulkUploadField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStockQty",
                table: "SKUItems",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultPurchaseQty",
                table: "SKUItems",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AddColumn<string>(
                name: "Column10",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column11",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column12",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column13",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column14",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column8",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Column9",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum10",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum11",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum12",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum13",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum8",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "headerColum9",
                schema: "temp",
                table: "ItemBulkUpload",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Column10",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column11",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column12",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column13",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column14",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column8",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "Column9",
                schema: "temp",
                table: "ItemBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: "headerColum10",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.DropColumn(
                name: "headerColum11",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.DropColumn(
                name: "headerColum12",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.DropColumn(
                name: "headerColum13",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.DropColumn(
                name: "headerColum8",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.DropColumn(
                name: "headerColum9",
                schema: "temp",
                table: "ItemBulkUpload");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStockQty",
                table: "SKUItems",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DefaultPurchaseQty",
                table: "SKUItems",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldNullable: true);
        }
    }
}
