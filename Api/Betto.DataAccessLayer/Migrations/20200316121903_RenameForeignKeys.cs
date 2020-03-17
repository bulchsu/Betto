using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class RenameForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Coverages_CoverageKey",
                table: "Leagues");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Leagues_LeagueEntityLeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeagueEntityLeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CoverageKey",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coverages",
                table: "Coverages");

            migrationBuilder.DropColumn(
                name: "LeagueEntityLeagueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CoverageKey",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "CoverageKey",
                table: "Coverages");

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverageId",
                table: "Leagues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverageId",
                table: "Coverages",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coverages",
                table: "Coverages",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues",
                column: "CoverageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Coverages_CoverageId",
                table: "Leagues",
                column: "CoverageId",
                principalTable: "Coverages",
                principalColumn: "CoverageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Leagues_LeagueId",
                table: "Teams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Coverages_CoverageId",
                table: "Leagues");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Leagues_LeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coverages",
                table: "Coverages");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CoverageId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "CoverageId",
                table: "Coverages");

            migrationBuilder.AddColumn<int>(
                name: "LeagueEntityLeagueId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoverageKey",
                table: "Leagues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoverageKey",
                table: "Coverages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coverages",
                table: "Coverages",
                column: "CoverageKey");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueEntityLeagueId",
                table: "Teams",
                column: "LeagueEntityLeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CoverageKey",
                table: "Leagues",
                column: "CoverageKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Coverages_CoverageKey",
                table: "Leagues",
                column: "CoverageKey",
                principalTable: "Coverages",
                principalColumn: "CoverageKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Leagues_LeagueEntityLeagueId",
                table: "Teams",
                column: "LeagueEntityLeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
