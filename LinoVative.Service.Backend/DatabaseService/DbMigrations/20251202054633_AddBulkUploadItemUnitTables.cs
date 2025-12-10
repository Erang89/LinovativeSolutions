using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddBulkUploadItemUnitTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemUnitBulkUpload",
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
                    table.PrimaryKey("PK_ItemUnitBulkUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnitBulkUploadDetail",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemUnitBulkUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Column1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column2 = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnitBulkUploadDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemUnitBulkUploadDetail_ItemUnitBulkUpload_ItemUnitBulkUploadId",
                        column: x => x.ItemUnitBulkUploadId,
                        principalSchema: "temp",
                        principalTable: "ItemUnitBulkUpload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitBulkUploadDetail_ItemUnitBulkUploadId",
                schema: "temp",
                table: "ItemUnitBulkUploadDetail",
                column: "ItemUnitBulkUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemUnitBulkUploadDetail",
                schema: "temp");

            migrationBuilder.DropTable(
                name: "ItemUnitBulkUpload",
                schema: "temp");
        }
    }
}
