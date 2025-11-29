using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class BulkUploadItemCategoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemCategoryBulkUpload",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    headerColum1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Operation = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategoryBulkUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategoryBulkUploadDetail",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCategoryBulkUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Column1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategoryBulkUploadDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCategoryBulkUploadDetail_ItemCategoryBulkUpload_ItemCategoryBulkUploadId",
                        column: x => x.ItemCategoryBulkUploadId,
                        principalSchema: "temp",
                        principalTable: "ItemCategoryBulkUpload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategoryBulkUploadDetail_ItemCategoryBulkUploadId",
                schema: "temp",
                table: "ItemCategoryBulkUploadDetail",
                column: "ItemCategoryBulkUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCategoryBulkUploadDetail",
                schema: "temp");

            migrationBuilder.DropTable(
                name: "ItemCategoryBulkUpload",
                schema: "temp");
        }
    }
}
