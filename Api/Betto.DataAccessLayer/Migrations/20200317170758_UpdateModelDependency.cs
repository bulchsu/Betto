using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class UpdateModelDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coverages_Fixtures_FixturesId",
                table: "Coverages");

            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Coverages_CoverageId",
                table: "Leagues");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Venues_VenueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_VenueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CoverageId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "FixturesId",
                table: "Coverages");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Venues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverageId",
                table: "Fixtures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Coverages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Venues_TeamId",
                table: "Venues",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_CoverageId",
                table: "Fixtures",
                column: "CoverageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_LeagueId",
                table: "Coverages",
                column: "LeagueId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coverages_Leagues_LeagueId",
                table: "Coverages",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Coverages_CoverageId",
                table: "Fixtures",
                column: "CoverageId",
                principalTable: "Coverages",
                principalColumn: "CoverageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venues_Teams_TeamId",
                table: "Venues",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coverages_Leagues_LeagueId",
                table: "Coverages");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Coverages_CoverageId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Venues_Teams_TeamId",
                table: "Venues");

            migrationBuilder.DropIndex(
                name: "IX_Venues_TeamId",
                table: "Venues");

            migrationBuilder.DropIndex(
                name: "IX_Fixtures_CoverageId",
                table: "Fixtures");

            migrationBuilder.DropIndex(
                name: "IX_Coverages_LeagueId",
                table: "Coverages");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CoverageId",
                table: "Fixtures");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Coverages");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverageId",
                table: "Leagues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FixturesId",
                table: "Coverages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_VenueId",
                table: "Teams",
                column: "VenueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues",
                column: "CoverageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages",
                column: "FixturesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coverages_Fixtures_FixturesId",
                table: "Coverages",
                column: "FixturesId",
                principalTable: "Fixtures",
                principalColumn: "FixturesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Coverages_CoverageId",
                table: "Leagues",
                column: "CoverageId",
                principalTable: "Coverages",
                principalColumn: "CoverageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Venues_VenueId",
                table: "Teams",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "VenueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
