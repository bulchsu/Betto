using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class RenameScoresProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("HalfTimeScore", "Scores", "HalfTime");
            migrationBuilder.RenameColumn("FullTimeScore", "Scores", "FullTime");
            migrationBuilder.RenameColumn("ExtraTimeScore", "Scores", "ExtraTime");
            migrationBuilder.RenameColumn("PenaltyScore", "Scores", "Penalty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("HalfTime", "Scores", "HalfTimeScore");
            migrationBuilder.RenameColumn("FullTime", "Scores", "FullTimeScore");
            migrationBuilder.RenameColumn("ExtraTime", "Scores", "ExtraTimeScore");
            migrationBuilder.RenameColumn("Penalty", "Scores", "PenaltyScore");
        }
    }
}
