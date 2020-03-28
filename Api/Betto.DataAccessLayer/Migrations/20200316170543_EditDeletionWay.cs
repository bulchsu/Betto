using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class EditDeletionWay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_VenueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_VenueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_VenueId",
                table: "Teams",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CoverageId",
                table: "Leagues",
                column: "CoverageId");

            migrationBuilder.CreateIndex(
                name: "IX_Coverages_FixturesId",
                table: "Coverages",
                column: "FixturesId");
        }
    }
}
