using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class AttachTeamsToLeague : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueEntityLeagueId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueEntityLeagueId",
                table: "Teams",
                column: "LeagueEntityLeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Leagues_LeagueEntityLeagueId",
                table: "Teams",
                column: "LeagueEntityLeagueId",
                principalTable: "Leagues",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Leagues_LeagueEntityLeagueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_LeagueEntityLeagueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LeagueEntityLeagueId",
                table: "Teams");
        }
    }
}
