using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    FixturesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Events = table.Column<bool>(nullable: false),
                    Lineups = table.Column<bool>(nullable: false),
                    Statistics = table.Column<bool>(nullable: false),
                    PlayerStatistics = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.FixturesId);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueName = table.Column<string>(maxLength: 200, nullable: false),
                    VenueSurface = table.Column<string>(maxLength: 100, nullable: false),
                    VenueAddress = table.Column<string>(maxLength: 200, nullable: false),
                    VenueCity = table.Column<string>(maxLength: 100, nullable: false),
                    VenueCapacity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                });

            migrationBuilder.CreateTable(
                name: "Coverages",
                columns: table => new
                {
                    CoverageKey = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Standings = table.Column<bool>(nullable: false),
                    FixturesId = table.Column<int>(nullable: false),
                    Players = table.Column<bool>(nullable: false),
                    TopScorers = table.Column<bool>(nullable: false),
                    Predictions = table.Column<bool>(nullable: false),
                    Odds = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coverages", x => x.CoverageKey);
                    table.ForeignKey(
                        name: "FK_Coverages_Fixtures_FixturesId",
                        column: x => x.FixturesId,
                        principalTable: "Fixtures",
                        principalColumn: "FixturesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Logo = table.Column<string>(maxLength: 300, nullable: false),
                    Country = table.Column<string>(maxLength: 100, nullable: false),
                    IsNational = table.Column<string>(nullable: false),
                    Founded = table.Column<int>(nullable: false),
                    VenueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    LeagueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: false),
                    Country = table.Column<string>(maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Season = table.Column<int>(nullable: false),
                    SeasonStart = table.Column<DateTime>(nullable: false),
                    SeasonEnd = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<string>(maxLength: 300, nullable: false),
                    Flag = table.Column<string>(maxLength: 300, nullable: false),
                    Standings = table.Column<bool>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    CoverageKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.LeagueId);
                    table.ForeignKey(
                        name: "FK_Leagues_Coverages_CoverageKey",
                        column: x => x.CoverageKey,
                        principalTable: "Coverages",
                        principalColumn: "CoverageKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages",
                column: "FixturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CoverageKey",
                table: "Leagues",
                column: "CoverageKey");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_VenueId",
                table: "Teams",
                column: "VenueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Coverages");

            migrationBuilder.DropTable(
                name: "Venues");

            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
