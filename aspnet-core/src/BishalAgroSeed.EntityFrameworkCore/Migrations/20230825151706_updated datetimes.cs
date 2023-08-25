using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BishalAgroSeed.Migrations
{
    /// <inheritdoc />
    public partial class updateddatetimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DateTimes_Datetime",
                table: "DateTimes",
                column: "Datetime",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DateTimes_Datetime",
                table: "DateTimes");
        }
    }
}
