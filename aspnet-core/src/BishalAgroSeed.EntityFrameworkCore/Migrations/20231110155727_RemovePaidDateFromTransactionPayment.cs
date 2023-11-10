using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BishalAgroSeed.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaidDateFromTransactionPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "TransactionPayments");

            migrationBuilder.AlterColumn<int>(
                name: "Cases",
                table: "TransactionDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "TransactionPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Cases",
                table: "TransactionDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
