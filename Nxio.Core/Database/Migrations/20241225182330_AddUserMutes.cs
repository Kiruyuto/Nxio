using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nxio.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuildSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MuteStartUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MuteEndUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RoleIdsBeforeMute = table.Column<string>(type: "nvarchar(max)", maxLength: 6000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMutes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildSettings");

            migrationBuilder.DropTable(
                name: "UserMutes");
        }
    }
}
