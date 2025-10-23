using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinoVative.Service.Backend.DatabaseService.DbMigrations
{
    /// <inheritdoc />
    public partial class UpdateAuditableTableUnderCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtcTime",
                table: "ItemUnits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ItemUnits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtcTime",
                table: "ItemUnits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "ItemUnits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtcTime",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtcTime",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtcTime",
                table: "ItemPriceTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ItemPriceTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemPriceTags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtcTime",
                table: "ItemPriceTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "ItemPriceTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtcTime",
                table: "ItemGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ItemGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtcTime",
                table: "ItemGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "ItemGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtcTime",
                table: "ItemCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ItemCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtcTime",
                table: "ItemCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "ItemCategories",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtcTime",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtcTime",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtcTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtcTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtcTime",
                table: "ItemPriceTags");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemPriceTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemPriceTags");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtcTime",
                table: "ItemPriceTags");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ItemPriceTags");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtcTime",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtcTime",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ItemGroups");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtcTime",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtcTime",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ItemCategories");
        }
    }
}
