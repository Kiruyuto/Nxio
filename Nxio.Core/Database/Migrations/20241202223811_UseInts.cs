using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nxio.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class UseInts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Upgrades");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Upgrades",
                type: "int",
                nullable: false,
                defaultValue: 2147483647,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ValuePerLevel",
                table: "Upgrades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValuePerLevel",
                table: "Upgrades");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Upgrades",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2147483647);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Upgrades",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
