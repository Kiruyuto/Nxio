using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nxio.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HitAttempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesMutedByOthers",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesMutedOthers",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinutesMutedSelf",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulHits",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitAttempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MinutesMutedByOthers",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MinutesMutedOthers",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MinutesMutedSelf",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuccessfulHits",
                table: "Users");
        }
    }
}
