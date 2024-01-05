using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BishalAgroSeed.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedInventoryCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "InventoryCounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "InventoryCounts",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "InventoryCounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "InventoryCounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "InventoryCounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "InventoryCounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "InventoryCounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "InventoryCounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "InventoryCounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "InventoryCounts",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
