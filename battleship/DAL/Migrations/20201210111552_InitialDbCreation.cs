using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameOptions",
                columns: table => new
                {
                    GameOptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    BoardWidth = table.Column<int>(nullable: false),
                    BoardHeight = table.Column<int>(nullable: false),
                    EBoatsCanTouch = table.Column<int>(nullable: false),
                    ENextMoveAfterHit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameOptions", x => x.GameOptionId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameOptionId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(maxLength: 128, nullable: true),
                    PlayerAId = table.Column<int>(nullable: false),
                    PlayerBId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_GameOptions_GameOptionId",
                        column: x => x.GameOptionId,
                        principalTable: "GameOptions",
                        principalColumn: "GameOptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    EPlayerType = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Boats",
                columns: table => new
                {
                    BoatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(nullable: false),
                    Inserted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boats", x => x.BoatId);
                    table.ForeignKey(
                        name: "FK_Boats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameBoats",
                columns: table => new
                {
                    GameBoatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    LifeCount = table.Column<int>(nullable: false),
                    Direction = table.Column<string>(nullable: false),
                    ShipId = table.Column<int>(nullable: false),
                    IsSunken = table.Column<bool>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoats", x => x.GameBoatId);
                    table.ForeignKey(
                        name: "FK_GameBoats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBoardStates",
                columns: table => new
                {
                    PlayerBoardStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    BoardState = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBoardStates", x => x.PlayerBoardStateId);
                    table.ForeignKey(
                        name: "FK_PlayerBoardStates_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boats_PlayerId",
                table: "Boats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoats_PlayerId",
                table: "GameBoats",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameOptionId",
                table: "Games",
                column: "GameOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerAId",
                table: "Games",
                column: "PlayerAId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerBId",
                table: "Games",
                column: "PlayerBId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBoardStates_PlayerId",
                table: "PlayerBoardStates",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_PlayerAId",
                table: "Games",
                column: "PlayerAId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_PlayerBId",
                table: "Games",
                column: "PlayerBId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_PlayerAId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_PlayerBId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Boats");

            migrationBuilder.DropTable(
                name: "GameBoats");

            migrationBuilder.DropTable(
                name: "PlayerBoardStates");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GameOptions");
        }
    }
}
