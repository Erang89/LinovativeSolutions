using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class RenameCountryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Country_CountryRegion_RegionId",
                table: "Country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CountryRegion",
                table: "CountryRegion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.RenameTable(
                name: "CountryRegion",
                newName: "CountryRegions");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.RenameIndex(
                name: "IX_Country_RegionId",
                table: "Countries",
                newName: "IX_Countries_RegionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountryRegions",
                table: "CountryRegions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Countries_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_CountryRegions_RegionId",
                table: "Countries",
                column: "RegionId",
                principalTable: "CountryRegions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Countries_CountryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_CountryRegions_RegionId",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CountryRegions",
                table: "CountryRegions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "CountryRegions",
                newName: "CountryRegion");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_RegionId",
                table: "Country",
                newName: "IX_Country_RegionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountryRegion",
                table: "CountryRegion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Country_CountryId",
                table: "Companies",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_CountryRegion_RegionId",
                table: "Country",
                column: "RegionId",
                principalTable: "CountryRegion",
                principalColumn: "Id");
        }
    }
}
