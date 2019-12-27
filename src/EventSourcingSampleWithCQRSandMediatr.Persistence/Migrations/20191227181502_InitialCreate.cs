using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    PlayerNumber = table.Column<int>(nullable: false),
                    ShowedCartAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fauls",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    PlayerNumber = table.Column<int>(nullable: false),
                    FauledAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fauls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HomeTeamId = table.Column<Guid>(nullable: false),
                    AwayTeamId = table.Column<Guid>(nullable: false),
                    StadiumName = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    EndedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    Team = table.Column<int>(nullable: false),
                    PlayerNumber = table.Column<int>(nullable: false),
                    ScoredAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_GameId",
                table: "Cards",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Fauls_GameId",
                table: "Fauls",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Id",
                table: "Games",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_GameId",
                table: "Scores",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Fauls");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Scores");
        }
    }
}
