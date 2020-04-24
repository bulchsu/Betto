using Microsoft.EntityFrameworkCore.Migrations;

namespace Betto.DataAccessLayer.Migrations
{
    public partial class AddRapidApiExternalIViewModelSomeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RapidApiExternalId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RapidApiExternalId",
                table: "Leagues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RapidApiExternalId",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RapidApiExternalId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "RapidApiExternalId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "RapidApiExternalId",
                table: "Games");
        }
    }
}
