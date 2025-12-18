using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddItemExceptionalToOutlet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutletItemExceptionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "Varchar(20)", nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OutletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedAtUtcTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletItemExceptionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutletItemExceptionals_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutletItemExceptionals_OutletId",
                table: "OutletItemExceptionals",
                column: "OutletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutletItemExceptionals");
        }
    }
}
