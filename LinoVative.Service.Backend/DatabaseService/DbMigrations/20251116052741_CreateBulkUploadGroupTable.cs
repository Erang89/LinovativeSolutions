using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class CreateBulkUploadGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "temp");

            migrationBuilder.CreateTable(
                name: "ItemGroupBulkUpload",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    headerColum1 = table.Column<string>(type: "varchar(100)", nullable: true),
                    headerColum2 = table.Column<string>(type: "varchar(100)", nullable: true),
                    CreatedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroupBulkUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroupBulkUploadDetail",
                schema: "temp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemGroupBulkUploadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Column1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Column2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroupBulkUploadDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGroupBulkUploadDetail_ItemGroupBulkUpload_ItemGroupBulkUploadId",
                        column: x => x.ItemGroupBulkUploadId,
                        principalSchema: "temp",
                        principalTable: "ItemGroupBulkUpload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroupBulkUploadDetail_ItemGroupBulkUploadId",
                schema: "temp",
                table: "ItemGroupBulkUploadDetail",
                column: "ItemGroupBulkUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemGroupBulkUploadDetail",
                schema: "temp");

            migrationBuilder.DropTable(
                name: "ItemGroupBulkUpload",
                schema: "temp");
        }
    }
}
