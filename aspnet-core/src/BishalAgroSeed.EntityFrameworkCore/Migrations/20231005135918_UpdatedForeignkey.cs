using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BishalAgroSeed.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CycleCounts_AbpUsers_CloseBy",
                table: "CycleCounts");

            migrationBuilder.DropIndex(
                name: "IX_CycleCounts_CloseBy",
                table: "CycleCounts");

            migrationBuilder.DropColumn(
                name: "CloseBy",
                table: "CycleCounts");

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_ClosedBy",
                table: "CycleCounts",
                column: "ClosedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CycleCounts_AbpUsers_ClosedBy",
                table: "CycleCounts",
                column: "ClosedBy",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CycleCounts_AbpUsers_ClosedBy",
                table: "CycleCounts");

            migrationBuilder.DropIndex(
                name: "IX_CycleCounts_ClosedBy",
                table: "CycleCounts");

            migrationBuilder.AddColumn<Guid>(
                name: "CloseBy",
                table: "CycleCounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CycleCounts_CloseBy",
                table: "CycleCounts",
                column: "CloseBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CycleCounts_AbpUsers_CloseBy",
                table: "CycleCounts",
                column: "CloseBy",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
