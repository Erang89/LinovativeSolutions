using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class AddRegencyLinkToCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAddress_Provinces_ProvinceId",
                table: "CustomerAddress");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_ProvinceId",
                table: "CustomerAddress");

            migrationBuilder.AddColumn<Guid>(
                name: "RegencyId",
                table: "CustomerAddress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_RegencyId",
                table: "CustomerAddress",
                column: "RegencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddress_Regencies_RegencyId",
                table: "CustomerAddress",
                column: "RegencyId",
                principalTable: "Regencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAddress_Regencies_RegencyId",
                table: "CustomerAddress");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_RegencyId",
                table: "CustomerAddress");

            migrationBuilder.DropColumn(
                name: "RegencyId",
                table: "CustomerAddress");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_ProvinceId",
                table: "CustomerAddress",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAddress_Provinces_ProvinceId",
                table: "CustomerAddress",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }
    }
}
