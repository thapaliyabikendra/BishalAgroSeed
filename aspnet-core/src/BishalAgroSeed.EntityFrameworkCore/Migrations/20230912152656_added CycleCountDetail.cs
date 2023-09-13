using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BishalAgroSeed.Migrations
{
    /// <inheritdoc />
    public partial class addedCycleCountDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CycleCounts_CycleCountNumbers_CycleCountNumberId",
                table: "CycleCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_CycleCounts_Products_ProductId",
                table: "CycleCounts");

            migrationBuilder.DropTable(
                name: "CycleCountNumbers");

            migrationBuilder.DropIndex(
                name: "IX_CycleCounts_CycleCountNumberId",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "CycleCountNumberId",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "PhysicalQuantity",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "SystemQuantity",
                table: "CycleCounts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CycleCounts",
                newName: "CloseBy");

            migrationBuilder.RenameIndex(
                name: "IX_CycleCounts_ProductId",
                table: "CycleCounts",
                newName: "IX_CycleCounts_CloseBy");

            migrationBuilder.AddColumn<string>(
                name: "CCINumber",
                table: "CycleCounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedBy",
                table: "CycleCounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "CycleCounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "CycleCounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CycleCountDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CycleCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhysicalQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CycleCountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CycleCountDetails_CycleCounts_CycleCountId",
                        column: x => x.CycleCountId,
                        principalTable: "CycleCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CycleCountDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountDetails_CycleCountId",
                table: "CycleCountDetails",
                column: "CycleCountId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountDetails_ProductId",
                table: "CycleCountDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CycleCounts_AbpUsers_CloseBy",
                table: "CycleCounts",
                column: "CloseBy",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CycleCounts_AbpUsers_CloseBy",
                table: "CycleCounts");

            migrationBuilder.DropTable(
                name: "CycleCountDetails");

            migrationBuilder.DropColumn(
                name: "CCINumber",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "CycleCounts");

            migrationBuilder.RenameColumn(
                name: "CloseBy",
                table: "CycleCounts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CycleCounts_CloseBy",
                table: "CycleCounts",
                newName: "IX_CycleCounts_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "CycleCountNumberId",
                table: "CycleCounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "PhysicalQuantity",
                table: "CycleCounts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "CycleCounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SystemQuantity",
                table: "CycleCounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CycleCountNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CloseBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CCINumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CycleCountNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CycleCountNumbers_AbpUsers_CloseBy",
                        column: x => x.CloseBy,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_CycleCountNumberId",
                table: "CycleCounts",
                column: "CycleCountNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCountNumbers_CloseBy",
                table: "CycleCountNumbers",
                column: "CloseBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CycleCounts_CycleCountNumbers_CycleCountNumberId",
                table: "CycleCounts",
                column: "CycleCountNumberId",
                principalTable: "CycleCountNumbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CycleCounts_Products_ProductId",
                table: "CycleCounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
