using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddBulkUploadItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemBulkUpload",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    headerColum1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum3 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum4 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum5 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum6 = table.Column<string>(type: "varchar(100)", nullable: true),
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
                    table.PrimaryKey("PK_ItemBulkUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemBulkUploadDetail",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemBulkUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Column1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column3 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column4 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column5 = table.Column<string>(type: "varchar(100)", nullable: true),
                    Column6 = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBulkUploadDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemBulkUploadDetail_ItemBulkUpload_ItemBulkUploadId",
                        column: x => x.ItemBulkUploadId,
                        principalSchema: "temp",
                        principalTable: "ItemBulkUpload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemBulkUploadDetail_ItemBulkUploadId",
                schema: "temp",
                table: "ItemBulkUploadDetail",
                column: "ItemBulkUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemBulkUploadDetail",
                schema: "temp");

            migrationBuilder.DropTable(
                name: "ItemBulkUpload",
                schema: "temp");
        }
    }
}
