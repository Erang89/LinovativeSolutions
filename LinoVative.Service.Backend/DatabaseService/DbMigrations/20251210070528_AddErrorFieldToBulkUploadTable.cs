using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddErrorFieldToBulkUploadTable : Migration
    {
        const string typeForErrorMessage = "varchar(1000)";
        const string errorField = "Errors";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: errorField,
                schema: "temp",
                table: "ItemUnitBulkUploadDetail",
                type: typeForErrorMessage,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: errorField,
                schema: "temp",
                table: "ItemGroupBulkUploadDetail",
                type: typeForErrorMessage,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: errorField,
                schema: "temp",
                table: "ItemCategoryBulkUploadDetail",
                type: typeForErrorMessage,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: errorField,
                schema: "temp",
                table: "ItemBulkUploadDetail",
                type: typeForErrorMessage,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: errorField,
                schema: "temp",
                table: "ItemUnitBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: errorField,
                schema: "temp",
                table: "ItemGroupBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: errorField,
                schema: "temp",
                table: "ItemCategoryBulkUploadDetail");

            migrationBuilder.DropColumn(
                name: errorField,
                schema: "temp",
                table: "ItemBulkUploadDetail");
        }
    }
}
