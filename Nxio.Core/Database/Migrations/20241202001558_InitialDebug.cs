using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nxio.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialDebug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoinMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MessageId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Coins = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoinMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinReactions_CoinMessages_CoinMessageId",
                        column: x => x.CoinMessageId,
                        principalTable: "CoinMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoinReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinMessages_GuildId_MessageId",
                table: "CoinMessages",
                columns: new[] { "GuildId", "MessageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinReactions_CoinMessageId",
                table: "CoinReactions",
                column: "CoinMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinReactions_UserId",
                table: "CoinReactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinReactions");

            migrationBuilder.DropTable(
                name: "CoinMessages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
